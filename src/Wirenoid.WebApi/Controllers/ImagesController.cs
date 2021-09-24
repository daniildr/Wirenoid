﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Docker.DotNet.Models;
using Wirenoid.Core.Interfaces;
using Wirenoid.WebApi.Controllers.Abstracts;

namespace Wirenoid.WebApi.Controllers
{
    [Route("api/docker/[controller]")]
    [ApiController]
    public class ImagesController : AbstractDockerController
    {
        public ImagesController(IDockerManager dockerManager)
            : base(dockerManager) { }

        [HttpGet(Order = 0)]
        public async Task<List<ImagesListResponse>> GetListOfImagesAsync() =>
            await _dockerManager.GetImagesListAsync();

        [HttpGet("{id}", Order = 1)]
        public async Task<ImagesListResponse> GetImageByIdAsync([NotNull] string id) =>
            await _dockerManager.GetImageByIdAsync(id);

        [HttpDelete("{id}", Order = 2)]
        public async Task DeleteImageAsync([NotNull] string id) =>
            await _dockerManager.DeleteImageAsync(id);

        [HttpGet("byName/{name}", Order = 3)]
        public async Task<ImagesListResponse> GetImageByNameAsync([NotNull] string name) =>
            await _dockerManager.GetImageByNameAsync(name);        

#nullable enable
        [HttpPost("create/", Order = 5)]
        public async Task<string> CreateImageAsync(string? image, string? tags)
        {
            if (string.IsNullOrEmpty(image) && string.IsNullOrEmpty(tags))
                return await _dockerManager.CreateImageAsync();
            else
            {
                _ = image ?? throw new ArgumentNullException(nameof(image));
                _ = tags ?? throw new ArgumentNullException(nameof(tags));
                return await _dockerManager.CreateImageAsync(image, tags);
            }
        }
#nullable disable
    }
}
