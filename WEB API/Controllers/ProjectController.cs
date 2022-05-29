﻿using BusinessLayer.Interfaces;
using WebApiCommon.Enums;
using DataAccessLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectServices _projectServices;
        public ProjectController(IProjectServices projectServices)
        {
            _projectServices = projectServices;
        }
        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetAllProjects()
        {
            try
            {
                return Ok(await _projectServices.GetAllProjects());
            }
            catch (System.Exception)
            {

                return StatusCode(500, "An error has occurred");
            }
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<ProjectDto>> GetProject(int id)
        {
            try
            {
                var result = await _projectServices.GetProject(id);
                if (result == null)
                {
                    return NotFound();
                }
                return result;
            }
            catch (System.Exception)
            {

                return StatusCode(500, "An error has occurred");
            }
        }
        [HttpPost("Create")]
        public async Task<ActionResult<ProjectDto>> CreateProject(ProjectDto project)
        {
            try
            {
                if (project == null)
                {
                    return BadRequest();
                }
                var projectModel = _projectServices.GetProjectByName(project.Name).Result;
                if (projectModel != null)
                {
                    return BadRequest();
                }
                else if (projectModel.StartDate > projectModel.CompletionDate)
                {
                    return BadRequest();
                }
                //zavrsi
                var createdProject = await _projectServices.AddProject(project);
                return createdProject;
            }
            catch (System.Exception)
            {
                return StatusCode(500, "An error has occurred");
            }
        }
        [HttpPut("Create/{Id}")]
        public async Task<ActionResult<ProjectDto>> UpdateProject(int id, ProjectDto project)
        {
            try
            {
                var projectToUpdate = await _projectServices.GetProject(id);
                //stavi uslove
                return await _projectServices.UpdateProject(id, project);
            }
            catch (System.Exception)
            {

                return StatusCode(500, "An error has occurred");
            }
        }
        [HttpDelete("Delete/{Id}")]
        public async Task<ActionResult<ProjectDto>> DeleteProject(int id)
        {
            try
            {
                var projectToDelete = await _projectServices.GetProject(id);
                //uslovi
                return await _projectServices.DeleteProject(id);
            }
            catch (System.Exception)
            {

                return StatusCode(500, "An error has occurred");
            }
        }
        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> Search(string name, int priority, ProjectStatus ProjectStatus, string sort)
        {
            try
            {
                var result = await _projectServices.Search(name, priority, ProjectStatus, sort);

                if (result.Any())
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (System.Exception)
            {

                return StatusCode(500, "An error has occurred");
            }
        }

        [HttpGet("{id:int}/Tasks")]
        public async Task<ActionResult<IEnumerable<DataAccessLayer.Model.TaskDto>>> FindAllTasks(int id)
        {
            try
            {
                var result = await _projectServices.FindAllTasks(id);


                if (result.Any())
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (System.Exception)
            {

                return StatusCode(500, "An error has occurred");
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<ProjectDto>> Patch(int id, JsonPatchDocument<ProjectDto> patchEntity)
        {

            try
            {
                return Ok(await _projectServices.UpdateProjectPatch(id, patchEntity));
            }

            catch
            {
                return StatusCode(500, "An error has occurred");
            }


        }
    }
}