using SAR.Control.Costume;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAR.Control.Recorder
{
    public class Recorder
    {
        public List<RecorderCommand> ReadFromFile(string path)
        {
            List<RecorderCommand> result = new List<RecorderCommand>();

            try
            {
                using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open)))
                {
                    string[] header;
                    string[] record;
                    try
                    {
                        header = reader.ReadLine().Split(';');
                    }
                    catch(Exception E)
                    {
                        throw new Exception($"Recorder: Невозможно обработать заголовок файла. {E.Message}");
                    }

                    TimeSpan _previousRecordTime = TimeSpan.FromMilliseconds(0);

                    while (reader.Peek() >= 0)
                    {
                        try
                        {
                            record = reader.ReadLine().Split(';');
                        }
                        catch (Exception E)
                        {
                            throw new Exception($"Recorder: Невозможно считать команду из файла. {E.Message}");
                        }

                        // Создать команду
                        RecorderCommand command = new RecorderCommand();

                        // Рассчитать длительность выполнения команды
                        TimeSpan _currentRecordTime = TimeSpan.FromMilliseconds(Convert.ToDouble(record[0]));
                        if (result.Count == 0)
                            command.Duration = TimeSpan.FromSeconds(2);
                        else
                            command.Duration = (_currentRecordTime - _previousRecordTime);

                        _previousRecordTime = _currentRecordTime;


                        // Получить данные об положении узлов.
                        List<CostumeJoint> joints = new List<CostumeJoint>();

                        for(int i=1;i<record.Length;i++)
                        {
                            try
                            {
                                joints.Add(new CostumeJoint
                                {
                                    Name = header[i],
                                    Value = Convert.ToSingle(record[i])
                                });
                            }
                            catch(Exception E)
                            {
                                throw new Exception($"Recorder: Невозможно обработать значение узла. {E.Message}");
                            }
                        }

                        command.Joints = joints;
                        result.Add(command);
                    }

                    return result;
                }
            }
            catch(Exception E)
            {
                throw new Exception($"Recorder: Ошибка при загрузке команд. {E.Message}");
            }
        }
    }
}
