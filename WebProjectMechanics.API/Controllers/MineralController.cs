using System;
using System.Collections.Generic;
using System.Linq;
using wpmMineralCollection;

namespace WebProjectMechanics.API.Controllers
    {
    public class MineralController : BaseApiController
        {
        public IEnumerable<MineralItem> Get(string id = null)
            {
            return new MineralItemList();
            }
        }
    }
