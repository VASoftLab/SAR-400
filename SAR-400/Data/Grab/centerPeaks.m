function out = centerPeaks(data,peak_min_value)

    % Определение количества считанных таблиц
    [~,tableCount]=size(data);
    [~,columnCount] = size(data{1});
    
    % Количество столбцов для выравнивания равно общему числу столбцов в
    % таблице минус 1 (т.к. исключаем столбец времени)
    columnCount = columnCount - 1;
    
    for c = 1:columnCount
        % Инициализация переменных
        avg_peak = zeros(1,tableCount);
        
        % Указатель на текущий столбец для выравнивания
        currentColumn = 1 + c;

        % Выбираем индекс середины пика для каждой таблицы
        for i=1:tableCount
            peakrows = find(data{i}{:,currentColumn} > peak_min_value);
            avg_peak(i) = round(mean(peakrows));
        end
        
        % Получаем индекс общей середины пика
        peak_center = round(mean(avg_peak));

        % Смещаем данные по индексам для приведения их к общей середине
        for i=1:tableCount
            % Считаем смещение в индексах
            diff = peak_center - avg_peak(i);
            
            
            % Смещение не требуется - переходим к следующей таблице
            if diff == 0
                continue;
            end
            
            % Если центр пика для данной таблице раньше, чем индекс общей
            % середины
            if diff > 0
                % Смещаем индексы в конец массива на diff значений
                data{i}{(1+diff):end,currentColumn} = data{i}{1:(end-diff),currentColumn};
                
                % Заполняем образовавшуюся пустоту в начале массива первой
                % точкой
                for j = 1:diff
                    data{i}{j,currentColumn} = data{i}{1+diff,currentColumn};
                end
            end
            
            if diff < 0
                % Получаем абсолютное значение разницы (необходимо для
                % того, чтобы избежать путаницы в знаках)
                diff = abs(diff);
                
                % Смещаем индексы в начало массива на diff значений
                data{i}{1:(end-diff),currentColumn} = data{i}{(1+diff):end,currentColumn};
                
                % Заполняем образовавшуюсь пустоту в конце массива
                % последней точкой
                for j = 1:diff
                    data{i}{(end - diff + j),currentColumn} = data{i}{(end-diff),currentColumn};
                end
            end
        end
    end
    
    % Возвращаем результат
    out = data;
end