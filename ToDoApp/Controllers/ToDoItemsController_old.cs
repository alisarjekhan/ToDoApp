using AutoMapper;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Web.Http;
using ToDoLib;

namespace ToDoApp.Controllers
{
    [RoutePrefix("api/ToDoItems")]
    public class ToDoItemsController : ApiController
    {
        IToDoRepoService _repoService; 
        public ToDoItemsController(IToDoRepoService repoService)
        {
            _repoService = repoService;
        }

        // GET api/<controller>
        [Route("")]
        public IHttpActionResult GetAllToDoItems()
        {
            try
            { 
                var allToDoItemsFromDB = _repoService.GetToDoItems();
                var allToDoItemDtos = Mapper.Map<IList<ToDoItemDto>>(allToDoItemsFromDB);
                return Ok(allToDoItemDtos);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET api/<controller>/5
        [Route("{toDoItemId:int}")]
        public IHttpActionResult GetToDoItemById(int toDoItemId)
        {
            try
            { 
                var toDoItemFromDB = _repoService.GetToDoItem(toDoItemId);

                if (toDoItemFromDB == null || toDoItemFromDB.IsDeleted)
                    return NotFound();

                var toDoItemDto = Mapper.Map<ToDoItemDto>(toDoItemFromDB);

                return Ok(toDoItemDto);

            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST api/<controller>
        [Route("")]
        public IHttpActionResult PostNewToDoItem([FromBody]ToDoItemDto todoItemDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid Data");

                var toDoItem = Mapper.Map<ToDoItem>(todoItemDto);
                _repoService.AddToDoItem(toDoItem);
                todoItemDto.ToDoItemId = toDoItem.ToDoItemId;
                return Created($"{Request.RequestUri}/{todoItemDto.ToDoItemId}", todoItemDto);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT api/<controller>/5
        [Route("toDoItemId:int")]
        public IHttpActionResult PutToDoItem(int toDoItemId, [FromBody]ToDoItemDto todoItemDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid Data");

                var toDoItem = Mapper.Map<ToDoItem>(todoItemDto);

                if (!_repoService.UpdateToDoItem(toDoItem))
                    return NotFound();

                return Ok();
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE api/<controller>/5
        [Route("toDoItemId:int")]
        public IHttpActionResult DeleteToDoItem(int toDoItemId)
        {
            try
            {
                if (!_repoService.DeleteToDoItem(toDoItemId))
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //[Route("{toDoItemId}/note/{toDoItemNoteId}")]
        //public IHttpActionResult GetNoteForToDoItem(int toDoItemId, int toDoItemNoteId)
        //{

        //}
    }
}