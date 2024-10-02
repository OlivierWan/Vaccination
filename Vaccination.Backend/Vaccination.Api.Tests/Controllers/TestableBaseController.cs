using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vaccination.Api.Controllers;

namespace Vaccination.Api.Tests.Controllers
{
    public class TestableBaseController : BaseController
    {
        public new string GetUserId()
        {
            return base.GetUserId();
        }
    }
}
