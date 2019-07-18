using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SARSimulatorControl
{
    public enum RobotAnswer
    {
        NoConnection = 0x00,
        ExceptionOccured = 0x01,
        SyntaxError = 0xFF,
        DeviceUnavailable = 0xFE,
        UnkonwnIndentifier = 0xFD,
        IncorrectInputValue = 0xFC,
        CommandExecuted = 0xF0,
        CommandExecutedWithReturnResult = 0xF1,
        CommandIsObsolete = 0xF2,
        CommandIsNotSupportedByDevice = 0xF3,
        CommandIsNotSupportedBySoftware = 0xF4,
        MaxAmountOfConnectionsAchieved = 0xFA
    }
}
