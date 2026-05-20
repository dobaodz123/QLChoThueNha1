using Microsoft.AspNetCore.Mvc;
using QIChoThueNha1.Models;

namespace QlChoThueNha1.Controllers
{
    public interface IChatController
    {
        Task<IActionResult> SendMessage([FromBody] ChatRequest request);
    }
}