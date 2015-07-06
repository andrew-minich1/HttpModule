$(function () {

    var showLoginForm = false;
    var loginIsValid = false;
    var passwordIsValid = false;

    $(document.body).on('click', 'a.login', function (event) {
        event.preventDefault();
        if (showLoginForm) {
            $(".login-conteiner").slideUp("fast");
            $('.loginForm').trigger('reset');
            showLoginForm = false;
        }
        else {
            $(".login-conteiner").slideDown("fast");
            showLoginForm = true;
        }
    });

    $(document.body).on('click', '.loginSubmit', function (event) {
        event.preventDefault();
        if (loginIsValid == false || passwordIsValid == false)
            return;
        var fields = $('.loginForm').serialize();
        $.ajax({
            type: "POST",
            url: '/Account/Login',
            data: fields,
            cache: false,
            error: function () {
                window.location = "/Account/Login";
            },
            success: function (response) {
                if (response == "") return;
                var result = '<ul class="nav navbar-nav navbar-right">'
                              + '<li class="dropdown">'
                                   + '<a href="#" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">' + response + '<span class="caret"></span></a>'
                                   + '<ul class="dropdown-menu" role="menu">'
                                       + '<li><a href="/Account/LogOff" class="logOff" >Exit</a></li>'
                                   + '</ul>'
                               + '</li>'
                           + '</ul>';

                $(".loginMenu").html(result);
                $(".login-conteiner").slideUp("fast");
                $('.loginForm').trigger('reset');
                showLoginForm = false;
            }
        })
    });

    $(document.body).on('click', '.logOff', function (event) {
        event.preventDefault();
        $.ajax({
            type: "GET",
            url: '/Account/LogOff',
            cache: false,
            success: function () {
                var result = '<div class="navbar-form navbar-right"> <a class=" btn btn-info login" href="/Account/Login">Login</a></div>';
                $(".loginMenu").html(result);
            }
        })
    });

    $('.loginForm input:text').blur(function (event) {
        if (this.value.length < 3) {
            this.classList.add("field-validation-error");
            loginIsValid = false;
        }
        else {
            this.classList.remove("field-validation-error");
            this.classList.add("field-validation-valid");
            loginIsValid = true;
        }
    });

    $('.loginForm input:password').blur(function (event) {
        if (this.value.length < 6) {
            this.classList.add("field-validation-error");
            passwordIsValid = false;
        }
        else {
            this.classList.remove("field-validation-error");
            this.classList.add("field-validation-valid");
            passwordIsValid = true;
        }
    });
});