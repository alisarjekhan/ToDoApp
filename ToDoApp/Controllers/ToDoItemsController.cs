using AutoMapper;
using DataLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Web.Http;
using ToDoLib;

namespace ToDoApp.Controllers
{
    [RoutePrefix("api/ToDoItems")]
    public class ToDoItemsController : ApiController
    {
        static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IToDoRepoService _repoService; 
        public ToDoItemsController(IToDoRepoService repoService)
        {
            _repoService = repoService;
        }

        [Route("")]
        public IHttpActionResult GetAllToDoItems()
        {
            try
            { 
                var allToDoItemsFromDB = _repoService.GetToDoItems();

                if (allToDoItemsFromDB.Count == 0)
                    return NotFound();

                var allToDoItemDtos = Mapper.Map<IList<ToDoItemDto>>(allToDoItemsFromDB);
                return Ok(allToDoItemDtos);
            }
            catch (Exception ex)
            {
                log.Error($"Error while executing {nameof(this.GetAllToDoItems)}", ex);
                return InternalServerError(ex);
            }
        }

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
                log.Error($"Error while executing {nameof(this.GetAllToDoItems)}", ex);
                return InternalServerError(ex);
            }
        }

        [Route("")]
        [Authorize]//(Roles = "admin")]
        public IHttpActionResult PostNewToDoItem([FromBody]ToDoItemDto todoItemDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid Data");

                if (!User.IsInRole("admin"))
                    return Unauthorized();

                var toDoItem = Mapper.Map<ToDoItem>(todoItemDto);
                _repoService.AddToDoItem(toDoItem);
                todoItemDto.ToDoItemId = toDoItem.ToDoItemId;
                return Created($"{Request.RequestUri}/{todoItemDto.ToDoItemId}", todoItemDto);
            }
            catch(Exception ex)
            {
                log.Error($"Error while executing {nameof(this.GetAllToDoItems)}", ex);
                return InternalServerError(ex);
            }
        }

        [Route("{toDoItemId:int}")]
        [Authorize]
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
                log.Error($"Error while executing {nameof(this.GetAllToDoItems)}", ex);
                return InternalServerError(ex);
            }
        }

        [Route("")]
        [Authorize]
        public IHttpActionResult PutToDoItems([FromBody] ToDoItemDto[] toDoItemDtos)
        {
            try
            {
                if (!ModelState.IsValid || toDoItemDtos == null || toDoItemDtos.Length == 0)
                    return BadRequest("Invalid Data");


                var toDoItems = new List<ToDoItem>();
                foreach (var toDoItemDto in toDoItemDtos)
                {
                    toDoItems.Add(Mapper.Map<ToDoItem>(toDoItemDto));
                }

                if (!_repoService.UpdateToDoItems(toDoItems))
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                log.Error($"Error while executing {nameof(this.GetAllToDoItems)}", ex);
                return InternalServerError(ex);
            }
        }

        [Route("{toDoItemId:int}")]
        [Authorize]
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
                log.Error($"Error while executing {nameof(this.GetAllToDoItems)}", ex);
                return InternalServerError(ex);
            }
        }

        [Route("{toDoItemId:int}/note")]
        [Authorize]
        public IHttpActionResult DeleteNoteForToDoItem(int toDoItemId)
        {
            try
            {
                if (!_repoService.DeleteNoteForToDoItem(toDoItemId))
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                log.Error($"Error while executing {nameof(this.GetAllToDoItems)}", ex);
                return InternalServerError(ex);
            }
        }

        [Route("{toDoItemId:int}/note")]
        [Authorize]
        public IHttpActionResult PostNoteToDoItem(int toDoItemId, [FromBody] string note)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid Data");

                if (!User.IsInRole("admin"))
                    return Unauthorized();

                var toDoItem = _repoService.GetToDoItem(toDoItemId);

                if (toDoItem == null)
                    return NotFound();

                if (toDoItem.ToDoItemNote != null)
                    return Conflict();

                _repoService.AddNoteForToDoItem(toDoItem, note);
                return Created($"{Request.RequestUri}", note);
            }
            catch (Exception ex)
            {
                log.Error($"Error while executing {nameof(this.GetAllToDoItems)}", ex);
                return InternalServerError(ex);
            }
        }

        [Route("{toDoItemId:int}/note")]
        public IHttpActionResult GetNoteForToDoItem(int toDoItemId)
        {
            try
            {
                var toDoItemFromDB = _repoService.GetToDoItem(toDoItemId);

                if (toDoItemFromDB == null || toDoItemFromDB.IsDeleted || toDoItemFromDB.ToDoItemNote == null)
                    return NotFound();

                return Ok(toDoItemFromDB.ToDoItemNote.Note);
            }
            catch (Exception ex)
            {
                log.Error($"Error while executing {nameof(this.GetAllToDoItems)}", ex);
                return InternalServerError(ex);
            }
        }

        [Route("{toDoItemId:int}/note")]
        [Authorize]
        public IHttpActionResult PutNoteForToDoItem(int toDoItemId, [FromBody]string note)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid Data");

                var toDoItemFromDB = _repoService.GetToDoItem(toDoItemId);

                if (toDoItemFromDB == null || toDoItemFromDB.IsDeleted || toDoItemFromDB.ToDoItemNote == null)
                    return NotFound();

                if (!_repoService.UpdateNoteToDoItem(toDoItemFromDB, note))
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                log.Error($"Error while executing {nameof(this.GetAllToDoItems)}", ex);
                return InternalServerError(ex);
            }
        }
    }
}