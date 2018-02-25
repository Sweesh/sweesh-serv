using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Sweesh.Core.Web.Controllers
{
    using System.Text;
    using Abstract.Adapters;
    using Models;

    public class FileController : BaseController
    {
        private IKeyValueFileAdapter keyAdapter;

        public FileController(IKeyValueFileAdapter keyAdapter)
        {
            this.keyAdapter = keyAdapter;
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            try
            {
                if (string.IsNullOrEmpty(UserId))
                {
                    return StatusCode(401, new
                    {
                        Worked = false,
                        Message = "Not authorized"
                    });
                }

                foreach(var file in files)
                {
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        await keyAdapter.Insert(new KeyValueFile
                        {
                            UserId = UserId,
                            Id = Guid.NewGuid().ToString(),
                            Key = file.Name,
                            Value = stream.ToArray(),
                            Mime = file.ContentType
                        });
                    }
                }

                return Ok(new
                {
                    Worked = true,
                    Message = "Files saved"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Worked = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("{key}"), Authorize]
        public async Task<IActionResult> Post([FromQuery]string key, [FromBody]string content)
        {
            try
            {
                if (string.IsNullOrEmpty(UserId))
                {
                    return StatusCode(401, new
                    {
                        Worked = false,
                        Message = "Not authorized"
                    });
                }

                var bytes = Encoding.UTF8.GetBytes(content);
                await keyAdapter.Insert(new KeyValueFile
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = UserId,
                    Key = key,
                    Value = bytes,
                    Mime = "raw"
                });

                return Ok(new
                {
                    Worked = true,
                    Message = "Files saved"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Worked = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{key}"), Authorize]
        public async Task<IActionResult> GetStream(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(UserId))
                {
                    return StatusCode(401, new
                    {
                        Worked = false,
                        Message = "Not authorized"
                    });
                }

                var file = await keyAdapter.GetByKey(key, UserId);

                if (file == null)
                {
                    return StatusCode(404, new
                    {
                        Worked = false,
                        Message = "Not found"
                    });
                }

                return File(file.Value, file.Mime);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Worked = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet("{key}"), Authorize]
        public async Task<IActionResult> Get(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(UserId))
                {
                    return StatusCode(401, new
                    {
                        Worked = false,
                        Message = "Not authorized"
                    });
                }

                var file = await keyAdapter.GetByKey(key, UserId);

                if (file == null)
                {
                    return StatusCode(404, new
                    {
                        Worked = false,
                        Message = "Not found"
                    });
                }

                return Ok(file.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Worked = false,
                    Message = ex.Message
                });
            }
        }
    }
}
