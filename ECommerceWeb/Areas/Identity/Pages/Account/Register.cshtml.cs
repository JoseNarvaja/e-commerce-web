// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using ECommerceWeb.DataAccess.Repository.Interfaces;
using ECommerceWeb.Models;
using ECommerceWeb.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace ECommerceWeb.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnviroment;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
            IWebHostEnvironment hostEnviroment)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _hostEnviroment = hostEnviroment;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "La contraseña debe tener entre {2} y {1} caracteres de longitud.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Este campo es obligatorio")]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "Este campo es obligatorio")]
            public string Apellido { get; set; }

            public string? Direccion { get; set; }
            public string? CodigoPostal { get; set; }
            public string? Provincia { get; set; }
            public string? Localidad { get; set; }


            [ValidateNever]
            public IEnumerable<SelectListItem> RoleList { get; set; }
            public string? Role { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            Input = new InputModel()
            {
                RoleList = _roleManager.Roles.Select(r => r.Name).Select(n => new SelectListItem()
                {
                    Text = n,
                    Value = n
                })
            };

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                user.Nombre= Input.Nombre;
                user.Apellido= Input.Apellido;
                user.Direccion= Input.Direccion;
                user.CodigoPostal= Input.CodigoPostal;
                user.Localidad = Input.Localidad;
                user.Provincia= Input.Provincia;
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    if (!String.IsNullOrEmpty(Input.Role)){
                        await _userManager.AddToRoleAsync(user, Input.Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.RolCliente);
                    }

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    string callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);


                    string pathTemplate = _hostEnviroment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                        + "Templates" + Path.DirectorySeparatorChar.ToString() + "ConfirmacionCorreo.html";

                    var htmlBody = "";
                    using(StreamReader streamReader = System.IO.File.OpenText(pathTemplate))
                    {
                        htmlBody = streamReader.ReadToEnd();
                    }
                    //HtmlEncoder.Default.Encode(callbackUrl)
                    string messageBody = string.Format(htmlBody,
                        Input.Nombre,
                        HtmlEncoder.Default.Encode(callbackUrl));


                    await _emailSender.SendEmailAsync(Input.Email, "Confirme su correo", messageBody);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        TempData["exito"] = "Nuevo usuario creado exitosamente";
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if (!User.IsInRole(SD.RolAdmin))
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                        }
                        else
                        {
                            TempData["exito"] = "Nuevo usuario creado exitosamente";
                        }
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private Usuario CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Usuario>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
