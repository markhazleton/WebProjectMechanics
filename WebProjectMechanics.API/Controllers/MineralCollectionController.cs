using System;
using System.Collections.Generic;
using System.Linq;
using wpmMineralCollection;

namespace WebProjectMechanics.API.Controllers
    {
    public class MineralCollectionController : BaseApiController
        {
        // GET api/values
        public IEnumerable<MineralCollectionItem> Get(string id = null)
            {
            var mySource = new MineralCollectionListView();
            if (!string.IsNullOrEmpty(id))
                mySource.SpecimenNumber = id;
            return mySource.GetList();
            }
        }
    }
