using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class BatteryModel
    {
        public double Voltage { get; set; }
        public double Current { get; set; }
        public double SOC { get; set; }
        public double MaxCellVoltage { get; set; }
        public double MinCellVoltage { get; set; }
        public double Temperature1 { get; set; }
        public double Temperature2 { get; set; }
        public bool ChargeMOSState { get; set; }
        public bool DischargeMOSState { get; set; }
    }
}
