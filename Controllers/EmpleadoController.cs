
using JerarquiaEmpleadosMVC.Models;
using Microsoft.AspNetCore.Mvc;


namespace JerarquiaEmpleadosMVC.Controllers

{
    public class EmpleadoController : Controller
    {
        private readonly HttpClient _http;

        public EmpleadoController(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient("BackendAPI");
        }


        // GET: /Empleado/Index
        public async Task<IActionResult> Index()
        {
            var listaPlano = await _http.GetFromJsonAsync<List<Empleado>>("api/Empleado/lista");
            var jerarquia = await _http.GetFromJsonAsync<List<Empleado>>("api/Empleado/jerarquia/arbol");

            var modelo = new EmpleadoViewModel
            {
                Lista = listaPlano ?? new List<Empleado>(),
                Jerarquia = jerarquia ?? new List<Empleado>()
            };

            return View(modelo);
        }



        // GET: /Empleado/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: /Empleado/Create
        [HttpPost]
        public async Task<IActionResult> Create(Empleado empleado)
        {
            var response = await _http.PostAsJsonAsync("api/Empleado", empleado);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError(string.Empty, "No se pudo insertar el empleado.");
            return View(empleado);
        }


        // GET: /Empleado/Details/{id}
        private Empleado? BuscarEmpleadoPorId(List<Empleado> lista, int id)
        {
            foreach (var empleado in lista)
            {
                if (empleado.Codigo == id)
                    return empleado;

                var encontrado = BuscarEmpleadoPorId(empleado.Subordinados, id);
                if (encontrado != null)
                    return encontrado;
            }

            return null;
        }


        // GET: /Empleado/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var empleados = await _http.GetFromJsonAsync<List<Empleado>>("api/Empleado/jerarquia/arbol");

            var item = BuscarEmpleadoPorId(empleados, id);

            if (item == null)
                return NotFound();

            return View(item);
        }



        // POST: /Empleado/Edit/{id}
        [HttpPost]
        public async Task<IActionResult> Edit(Empleado empleado)
        {
            var response = await _http.PutAsJsonAsync($"api/Empleado/{empleado.Codigo}", empleado);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError(string.Empty, "No se pudo actualizar el empleado.");
            return View(empleado);
        }


        // GET: /Empleado/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _http.DeleteAsync($"api/Empleado/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return BadRequest("No se pudo eliminar el empleado.");
        }


           
    }
}
