using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SuperHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {

        List<SuperHero> heroes = new List<SuperHero>
            {
                new SuperHero{ Id=1 ,  Name="Spider Man", FirstName="Peter" , LastName= "Parker" , Place="New York City"},
                new SuperHero{ Id=2 ,  Name="Batmam", FirstName="Bruce" , LastName= "Wayne" , Place="Gothem City"},

            };
        private DataContext context;

        public SuperHeroController (DataContext context)

	{
            this.context = context;
	}

        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> Get()
        {           
            return Ok(await context.SuperHeroes.ToListAsync());
        }




        [HttpGet]
        [Route("find")]
        public async Task<ActionResult<List<SuperHero>>> Get(int id)
        {
            if(id == 0)
            {
                return BadRequest("Please provide a valid super hero id");
            }

            SuperHero? selectedSuperHero = new SuperHero();

            selectedSuperHero = await this.context.SuperHeroes.FindAsync(id);

            if (selectedSuperHero == null)
                return BadRequest("Please provide a valid super hero id");

            return Ok(selectedSuperHero);
        }



        [HttpPost]
        [Route("Add")]

        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {
            this.context.SuperHeroes.Add(hero);
            await this.context.SaveChangesAsync();

            return Ok(await this.context.SuperHeroes.ToListAsync());
                
           
        }



        [HttpPut]
        [Route("Edit")]

        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero request)
        {


            var dbhero = await this.context.SuperHeroes.FindAsync(request.Id);
            if (dbhero == null)
            {
                return BadRequest("Please provide a valid super hero id");
            }
           

            dbhero.Name= request.Name;    
            dbhero.FirstName= request.FirstName; 
            dbhero.LastName= request.LastName;    
            dbhero.Place= request.Place;


            await this.context.SaveChangesAsync();

            return Ok(await this.context.SuperHeroes.ToListAsync());
        }


        [HttpDelete("{id}")]

        public async Task<ActionResult<List<SuperHero>>> Delete(int id)
        {

          var dbhero = await this.context.SuperHeroes.FindAsync(id);
            if (dbhero == null)
                return BadRequest("Hero Not Found");

            this.context.SuperHeroes.Remove(dbhero);
            await this.context.SaveChangesAsync();
            return Ok(await this.context.SuperHeroes.ToListAsync());    






        }







    }
}
