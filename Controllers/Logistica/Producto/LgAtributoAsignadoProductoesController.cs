using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_intiSoft.Models.Logistica.Producto;
using intiSoft;

namespace api_intiSoft.Controllers.Logistica.Producto
{
    [Route("api/[controller]")]
    [ApiController]
    public class LgAtributoAsignadoProductoesController : ControllerBase
    {
        private readonly ConecDinamicaContext _context;

        public LgAtributoAsignadoProductoesController(ConecDinamicaContext context)
        {
            _context = context;
        }

        

       

        
      
    }
}
