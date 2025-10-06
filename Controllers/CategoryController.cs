//using MediatR;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using OnlineExam.Features.Categories.Commands;
//using OnlineExam.Features.Categories.Dtos;
//using OnlineExam.Features.Categories.Queries;

//namespace OnlineExam.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CategoryController(IMediator _mediator) : ControllerBase
//    {

//        #region get category by id 

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetCategoryById(int id)
//        {
//            var category = await _mediator.Send(new GetCategoryByIdQuery(id));
//            if (category == null)
//                return NotFound(new { Message = $"Category with id {id} not found" });
//            return Ok(category);
//        }

//        #endregion



//        #region create category

//        [HttpPost]
//        public async Task<IActionResult> Create([FromForm] createCategoryDTo createCategoryDTo)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);
//            var categoryId = await _mediator.Send(new CreateCategoryCommand(createCategoryDTo));
//            return Ok(new { id = categoryId, title = createCategoryDTo.Title });
//        }
//        #endregion


//        #region update category

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Edit(int id, [FromForm] UpdateCategoryDTo editCategoryDTo)
//        {
//            if (!ModelState.IsValid)
//                return BadRequest(ModelState);

//            var categoryId = await _mediator.Send(new UpdateCategoryCommand(id, editCategoryDTo));
//            return Ok(new { id = categoryId, title = editCategoryDTo.Title });
//        }

//        #endregion



//        #region delete category

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var result = await _mediator.Send(new DeleteCategoryCommand(id));
//            if (!result)
//                return NotFound();

//            return Ok(result);
//        }

//        #endregion


//        #region get category for admin


//        [HttpGet]
//        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10,
//                                        [FromQuery] string? search = null, [FromQuery] string? sortBy = null)
//        {
//            var categories = await _mediator.Send(new GetCategoriesQueryForAdmin(pageNumber, pageSize, search, sortBy));
//            return Ok(categories);
//        }

//        #endregion


//        #region get category for user 

//        [HttpGet("user")]
//        public async Task<IActionResult> GetUserCategories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
//        {
//            var categories = await _mediator.Send(new GetUserCategoriesQuery(pageNumber, pageSize));

//            return Ok(categories);
//        }

//        #endregion






//    }
//}
