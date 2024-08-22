using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication2.Models;
using Microsoft.Extensions.Logging;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.UserNameOrEmail)
                       ?? await _userManager.FindByEmailAsync(model.UserNameOrEmail);

            if (user != null && user.SecurityQuestion == model.SecurityQuestion && user.SecurityAnswer == model.SecurityAnswer)
            {
                var resetPasswordViewModel = new ResetPasswordBySecurityQuestionViewModel
                {
                    UserNameOrEmail = model.UserNameOrEmail,
                    SecurityQuestion = user.SecurityQuestion
                };
                return View("ResetPasswordBySecurityQuestion", resetPasswordViewModel);
            }

            ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı, e-posta veya güvenlik sorusu cevabı.");
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult ResetPasswordBySecurityQuestion()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPasswordBySecurityQuestion(ResetPasswordBySecurityQuestionViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(model.UserNameOrEmail)
                       ?? await _userManager.FindByEmailAsync(model.UserNameOrEmail);

            if (user != null && user.SecurityQuestion == model.SecurityQuestion && user.SecurityAnswer == model.SecurityAnswer)
            {
                var newPasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
                user.PasswordHash = newPasswordHash;

                var updateResult = await _userManager.UpdateAsync(user);

                if (updateResult.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }

                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı, e-posta veya güvenlik sorusu cevabı.");
            }
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(model.UserNameOrEmail, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            // Yönlendirme
            return RedirectToAction("Index","Library");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Geçersiz giriş giriş.");
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                SecurityQuestion = model.SecurityQuestion,
                SecurityAnswer = model.SecurityAnswer
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction("Index", "Library");
        }
    }
}
