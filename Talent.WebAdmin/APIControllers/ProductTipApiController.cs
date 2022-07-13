using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Talent.WebAdmin.Models;
using Talent.WebAdmin.Services;

namespace Talent.WebAdmin.APIControllers
{
    [Route("api/v1/ProductTips")]
    public class ProductTipApiController : Controller
    {
        private readonly ProductTipService ProductTipMan;

        public ProductTipApiController(ProductTipService productTipService)
        {
            this.ProductTipMan = productTipService;
        }

        [HttpGet("get-all-product-tips")]
        public async Task<ActionResult<ProductTipPaginate>> GetAllProductTipAsync([FromQuery] ProductTipGridFilter filter)
        {
            var data = await this.ProductTipMan.GetAllProductTip(filter);
            return Ok(data);
        }

        [HttpPost("insert-product-tip", Name = "InsertProductTipAsync")]
        public async Task<ActionResult<BaseResponse>> InsertProductTipAsync([FromBody] ProductTipCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductTipMan.InsertProductTipAsync(model);

            return success;
        }

        [HttpGet("get-product-tip-by-id", Name = "GetProductTipByIdAsync")]
        public async Task<ActionResult<ProductTipModel>> GetProductTipByIdAsync([FromQuery] Guid ProductTipId)
        {
            var data = await this.ProductTipMan.GetProductTipById(ProductTipId);

            return Ok(data);
        }

        [HttpPost("update-product-tip-by-id", Name = "UpdateProductTipAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductTipAsync([FromBody] ProductTipUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var success = await this.ProductTipMan.UpdateProductTipAsync(model);

            return success;
        }

        [HttpDelete("delete-product-tip", Name = "DeleteProductTipAsync")]
        public async Task<ActionResult<BaseResponse>> DeleteProductTipAsync([FromBody] DeleteProductTipModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductTipMan.DeleteProductTipAsync(model);

            return getData;
        }

        [HttpPost("update-product-tip-by-approval", Name = "UpdateProductTipApprovalAsync")]
        public async Task<ActionResult<BaseResponse>> UpdateProductTipByApprovalAsync([FromBody] UpdateProductTipApproval model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var getData = await this.ProductTipMan.UpdateProductTipByApprovalAsync(model);

            return getData;
        }
    }
}
