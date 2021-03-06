﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hogwarts.Data.Models;
using Hogwarts.Api.Services;
using Hogwarts.Api.Services.Interfaces;
using Hogwarts.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hogwarts.Api.Controllers
{
    [Route("api/Roles")]
    [ApiController]
    public class RolesController : Controller
    {
        IRoleRepository _roleRepository;
        private IStaffRepository _staffRepository;
        IMapper _mapper;
        public RolesController(IMapper mapper, IRoleRepository roleRepository,
            IStaffRepository staffRepository)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _staffRepository = staffRepository;
        }
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            var roleEntities = await _roleRepository.GetRolesAsync();
            if (roleEntities == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<RoleDto>>(roleEntities));
        }
        // GET api/roles/5
        [HttpGet("{id}", Name = "GetRole")]
        public async Task<ActionResult<RoleDto>> GetRole(int id)
        {
            var roleEntity = await _roleRepository.GetRoleByIdAsync(id);
            if (roleEntity == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<RoleDto>(roleEntity));
        }
        // POST: api/roles
        [HttpPost]
        public async Task<ActionResult<RoleDto>> CreateRole(
            [FromBody] RoleForCreationDto roleForCreation)
        {
            var roleEntity = _mapper.Map<Role>(roleForCreation);
            _roleRepository.AddRole(roleEntity);
            await _roleRepository.SaveAsync();
            var roleToReturn = _mapper.Map<RoleDto>(roleEntity);
            return CreatedAtRoute("GetRole", new { staffId = roleToReturn.Id }, roleToReturn);
        }

        [HttpPatch("{roleId}")]
        public async Task<ActionResult<RoleDto>> PartiallyUpdateRole(int roleId,
            JsonPatchDocument<RoleForEditDto> patchDocument)
        {
            var roleFromRepo = await _roleRepository.GetRoleByIdAsync(roleId);
            if (roleFromRepo == null)
            {
                return NotFound();
            }
            var roleToPatch = _mapper.Map<RoleForEditDto>(roleFromRepo);
            patchDocument.ApplyTo(roleToPatch, ModelState);
            if(!TryValidateModel(roleToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(roleToPatch, roleFromRepo);
            _roleRepository.UpdateRole(roleId);
            await _roleRepository.SaveAsync();
            return Ok(roleFromRepo);
        }
        [HttpDelete("{roleId}")]
        public async Task<ActionResult> DeleteRole(int staffId)
        {
            var roleFromRepo = await _roleRepository.GetRoleByIdAsync(staffId);
            if (roleFromRepo == null)
            {
                return NotFound();
            }
            _roleRepository.DeleteRole(roleFromRepo);
            await _roleRepository.SaveAsync();
            return NoContent();
        }

    }
}
