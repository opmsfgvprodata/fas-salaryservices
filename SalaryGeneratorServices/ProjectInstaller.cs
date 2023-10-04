using SalaryGeneratorServices.FuncClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryGeneratorServices
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            string ServicesName = ConfigReader("configs", "servicename") + "_" + ConfigReader("configs", "negaraid") + "_" + ConfigReader("configs", "syarikatid") + "_" + ConfigReader("configs", "wilayahid") + "_" + ConfigReader("configs", "ladangid") + "_" + ConfigReader("configs", "divisionid");

            this.serviceInstaller1.ServiceName = ServicesName;
            this.serviceInstaller1.DisplayName = ServicesName;
        }

        private string ConfigReader(string Name, string Data)
        {
            string getresult = "";
            INIReaderFunc parser = new INIReaderFunc(AppDomain.CurrentDomain.BaseDirectory + "Configs.ini");

            getresult = parser.GetSetting(Name, Data);

            return getresult;
        }
    }
}
