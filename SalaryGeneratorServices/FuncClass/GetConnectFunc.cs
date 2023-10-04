using SalaryGeneratorServices.ModelsHQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryGeneratorServices.FuncClass
{
    class GetConnectFunc
    {
        public void GetConnection(out string host, out string catalog, out string user, out string pass, int? wlyhID, int? syrktID, int? ngrID)
        {
            GenSalaryModelHQ db = new GenSalaryModelHQ();
            var getconnection = db.tblConnections.Where(x => x.wilayahID == wlyhID && x.syarikatID == syrktID && x.negaraID == ngrID).FirstOrDefault();
            host = getconnection.DataSource;
            catalog = getconnection.InitialCatalog;
            user = getconnection.userID;
            pass = getconnection.Password;

        }
    }
}
