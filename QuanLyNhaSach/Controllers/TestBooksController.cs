using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuanLyNhaSach.Data;

namespace QuanLyNhaSach.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class TestBooksController(QuanLyBanSachContext context) : ControllerBase {
    }
}
