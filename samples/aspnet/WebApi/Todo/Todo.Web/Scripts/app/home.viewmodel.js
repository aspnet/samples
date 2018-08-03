function HomeViewModel(app, dataModel) {
    var self = this;

    /// <field name="todoLists" value="[new todoList()]"></field>
    self.todoLists = ko.observableArray();
    self.loading = ko.observable(true);

    function showTodoList(todoList) {
        self.todoLists.unshift(todoList); // Insert new todoList at the front
    }

    self.addTodoList = function () {
        var list = new todoList(dataModel); // todoList is injected by todo.model.js
        list.isEditingListTitle(true);
        list.errorMessage(null);
        dataModel.saveNewTodoList(list.toJS())
            .done(addSucceeded)
            .fail(addFailed);

        function addSucceeded(result) {
            list.todoListId = result.TodoListId;
            list.userId = result.UserId;
            showTodoList(list);
        }

        function addFailed() {
            list.errorMessage("Error adding a new todo list.");
        }
    };

    self.deleteTodoList = function (todoList) {
        self.todoLists.remove(todoList);
        dataModel.deleteTodoList(todoList.toJS())
            .fail(deleteFailed);

        function deleteFailed() {
            todoList.errorMessage("Error removing todo list.");
            self.showTodoList(todoList); // re-show the restored list
        }
    };

    dataModel.getTodoLists()
        .done(function (data) {
            self.loading(false);
            var mappedTodoLists = $.map(data.value, function (list) {
                return new todoList(dataModel, list); // todoList is injected by todo.model.js
            });
            self.todoLists(mappedTodoLists);
        }).fail(function (xhr) {
            if (xhr.status === 401)
                app.navigateToLogin();
            else {
                self.loading(false);
                app.errors.push("Error retrieving todo lists.");
            }
        }); // load todoLists
}

app.addViewModel({
    name: "Home",
    bindingMemberName: "home",
    factory: HomeViewModel
});