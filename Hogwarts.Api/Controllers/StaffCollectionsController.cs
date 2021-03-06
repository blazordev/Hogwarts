﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hogwarts.Api.Helpers;
using Hogwarts.Data.Models;
using Hogwarts.Api.Services;
using Hogwarts.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hogwarts.Api.Controllers
{
    [Route("api/StaffCollections")]
    [ApiController]
    public class StaffCollectionsController : ControllerBase
    {
        private IMapper _mapper;
        private IStaffRepository _staffRepo;

        public StaffCollectionsController(IMapper mapper, IStaffRepository staffRepo)
        {
            _mapper = mapper;
            _staffRepo = staffRepo;
        }

        //DELETE api/StaffCollections/id,id,id
        [HttpDelete("({ids})")]
        public async Task<ActionResult> DeleteStaffCollection([FromRoute]
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }
            var staffEntities = await _staffRepo.GetStaffCollectionAsync(ids);

            if (ids.Count() != staffEntities.Count())
            {
                return NotFound();
            }
            _staffRepo.DeleteStaffCollection(staffEntities);
            await _staffRepo.SaveAsync();
            var count = staffEntities.Count();
            if(count == 1)
            {
                return Ok($"{count} record deleted");
            }
            else
            {
                return Ok($"{count} records deleted");
            }
        }
        [HttpPut]
        public async Task<ActionResult> UpdateStaffCollection([FromBody] IEnumerable<StaffDto> staffCollection)
        {
            if (staffCollection == null || staffCollection.Count() == 0)
            {
                return BadRequest();
            }
            foreach (var staffItem in staffCollection)
            {
                var staffToUpdate = await _staffRepo.GetStaffByIdAsync(staffItem.Id);
                if (staffToUpdate == null)
                {
                    return NotFound();
                }
                _mapper.Map(staffItem, staffToUpdate);
            }
            await _staffRepo.SaveAsync();
            return Ok("Update successful");
        }
    }
}
