using MonolithBoilerPlate.Service.Base;
using Microsoft.AspNetCore.Mvc;

namespace MonolithBoilerPlate.Api.Controllers.Base
{
    public class GenericBaseController<T> : ControllerBase where T : class
    {
        protected readonly IBaseService<T> _service;

        public GenericBaseController(IBaseService<T> service)
        {
            _service = service;
        }

        [HttpGet("find/{id}")]
        public async Task<IActionResult> FindAsync(long id)
        {
            return Ok(await _service.FindAsync(id));
        }

        [HttpGet("get/{id}")]
        public virtual async Task<IActionResult> GetAsync(long id)
        {
            return Ok(await _service.FirstOrDefaultAsync(id));
        }

        [HttpGet("gets")]
        public virtual async Task<IActionResult> GetsAsync()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost("add")]
        public virtual async Task<IActionResult> AddAsync(T entity)
        {
            var res = await _service.InsertAsync(entity);
            return Created("", res);
        }

        [HttpPut("edit/{id}")]
        public virtual async Task<IActionResult> EditAsync(long id, T entity)
        {
            var res = await _service.UpdateAsync(id, entity);
            return Ok(res);
        }

        [HttpDelete("delete/{id}")]
        public virtual async Task<IActionResult> DeleteAsync(long id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("delete")]
        public virtual async Task<IActionResult> DeleteAsync(T entity)
        {        
            Type type = entity.GetType();
            long Id = (long)type.GetProperty("Id").GetValue(entity);
            await _service.DeleteAsync(Id);
            return NoContent();
        }
    }
}
