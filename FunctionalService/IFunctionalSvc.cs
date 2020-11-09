using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalService
{
    public interface IFunctionalSvc
    {
        public Task CreateDefaultAdminUser();
        public Task CreateDefaultAppUser();
    }
}
