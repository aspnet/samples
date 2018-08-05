function AppDataModel() {
    var self = this,
        // Routes
        addExternalLoginUrl = "/api/Account/AddExternalLogin",
        changePasswordUrl = "/api/Account/changePassword",
        loginUrl = "/Token",
        logoutUrl = "/api/Account/Logout",
        registerUrl = "/api/Account/Register",
        registerExternalUrl = "/api/Account/RegisterExternal",
        removeLoginUrl = "/api/Account/RemoveLogin",
        setPasswordUrl = "/api/Account/setPassword",
        siteUrl = "/",
        userInfoUrl = "/api/Account/UserInfo";

    function todoListUrl(id) { return "/odata/TodoLists(" + (id || "") + ")/"; }
    function todoItemUrl(id) { return "/odata/Todos(" + (id || "") + ")/"; }

    // Route operations
    function externalLoginsUrl(returnUrl, generateState) {
        return "/api/Account/ExternalLogins?returnUrl=" + (encodeURIComponent(returnUrl)) +
            "&generateState=" + (generateState ? "true" : "false");
    }

    function manageInfoUrl(returnUrl, generateState) {
        return "/api/Account/ManageInfo?returnUrl=" + (encodeURIComponent(returnUrl)) +
            "&generateState=" + (generateState ? "true" : "false");
    }

    // Other private operations
    function getSecurityHeaders() {
        var accessToken = sessionStorage["accessToken"] || localStorage["accessToken"];

        if (accessToken) {
            return { "Authorization": "Bearer " + accessToken };
        }

        return {};
    }

    // Operations
    self.clearAccessToken = function () {
        localStorage.removeItem("accessToken");
        sessionStorage.removeItem("accessToken");
    };

    self.setAccessToken = function (accessToken, persistent) {
        if (persistent) {
            localStorage["accessToken"] = accessToken;
        } else {
            sessionStorage["accessToken"] = accessToken;
        }
    };

    self.toErrorsArray = function (data) {
        var errors = new Array(),
            items;

        if (!data || !data.message) {
            return null;
        }

        if (data.modelState) {
            for (var key in data.modelState) {
                items = data.modelState[key];

                if (items.length) {
                    for (var i = 0; i < items.length; i++) {
                        errors.push(items[i]);
                    }
                }
            }
        }

        if (errors.length === 0) {
            errors.push(data.message);
        }

        return errors;
    };

    // Data
    self.returnUrl = siteUrl;

    // Data access operations
    self.addExternalLogin = function (data) {
        return $.ajax(addExternalLoginUrl, {
            type: "POST",
            data: data,
            headers: getSecurityHeaders()
        });
    };

    self.changePassword = function (data) {
        return $.ajax(changePasswordUrl, {
            type: "POST",
            data: data,
            headers: getSecurityHeaders()
        });
    };

    self.getExternalLogins = function (returnUrl, generateState) {
        return $.ajax(externalLoginsUrl(returnUrl, generateState), {
            cache: false,
            headers: getSecurityHeaders()
        });
    };

    self.getManageInfo = function (returnUrl, generateState) {
        return $.ajax(manageInfoUrl(returnUrl, generateState), {
            cache: false,
            headers: getSecurityHeaders()
        });
    };

    self.getUserInfo = function (accessToken) {
        var headers;

        if (typeof (accessToken) !== "undefined") {
            headers = {
                "Authorization": "Bearer " + accessToken
            };
        } else {
            headers = getSecurityHeaders();
        }

        return $.ajax(userInfoUrl, {
            cache: false,
            headers: headers
        });
    };

    self.login = function (data) {
        return $.ajax(loginUrl, {
            type: "POST",
            data: data
        });
    };

    self.logout = function () {
        return $.ajax(logoutUrl, {
            type: "POST",
            headers: getSecurityHeaders()
        });
    };

    self.register = function (data) {
        return $.ajax(registerUrl, {
            type: "POST",
            data: data
        });
    };

    self.registerExternal = function (accessToken, data) {
        return $.ajax(registerExternalUrl, {
            type: "POST",
            data: data,
            headers: {
                "Authorization": "Bearer " + accessToken
            }
        });
    };

    self.removeLogin = function (data) {
        return $.ajax(removeLoginUrl, {
            type: "POST",
            data: data,
            headers: getSecurityHeaders()
        });
    };

    self.setPassword = function (data) {
        return $.ajax(setPasswordUrl, {
            type: "POST",
            data: data,
            headers: getSecurityHeaders()
        });
    };

    // Todo operations
    self.getTodoLists = function () {
        return $.ajax(todoListUrl() + "?$expand=Todos", {
            cache: false,
            headers: getSecurityHeaders()
        });
    };

    self.saveChangedTodoItem = function (todoItem) {
        return $.ajax(todoItemUrl(todoItem.TodoItemId), {
            type: "PATCH",
            data: JSON.stringify(todoItem),
            contentType: "application/json",
            headers: getSecurityHeaders()
        });
    };

    self.saveChangedTodoList = function (todoList) {
        return $.ajax(todoListUrl(todoList.TodoListId), {
            type: "PATCH",
            data: JSON.stringify(todoList),
            contentType: "application/json",
            headers: getSecurityHeaders()
        });
    };

    self.saveNewTodoItem = function (todoItem) {
        return $.ajax(todoItemUrl(), {
            type: "POST",
            data: JSON.stringify(todoItem),
            contentType: "application/json",
            headers: getSecurityHeaders()
        });
    };

    self.saveNewTodoList = function (todoList) {
        return $.ajax(todoListUrl(), {
            type: "POST",
            data: JSON.stringify(todoList),
            contentType: "application/json",
            headers: getSecurityHeaders()
        });
    };

    self.deleteTodoItem = function (todoItem) {
        return $.ajax(todoItemUrl(todoItem.TodoItemId), {
            type: "DELETE",
            headers: getSecurityHeaders()
        });
    };

    self.deleteTodoList = function (todoList) {
        return $.ajax(todoListUrl(todoList.TodoListId), {
            type: "DELETE",
            headers: getSecurityHeaders()
        });
    };

}
