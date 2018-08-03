function todoItem(dataModel, data) {
    var self = this;
    data = data || {};

    // Persisted properties
    self.todoItemId = data.TodoItemId;
    self.title = ko.observable(data.Title);
    self.isDone = ko.observable(data.IsDone);
    self.todoListId = data.TodoListId;

    // Non-persisted properties
    self.errorMessage = ko.observable();

    function saveChanges() {
        self.errorMessage(null);
        return dataModel.saveChangedTodoItem(self.toJS())
            .fail(function () {
                self.errorMessage("Error updating todo item.");
            });
    }

    // Auto-save when these properties change
    self.isDone.subscribe(saveChanges);
    self.title.subscribe(saveChanges);

    self.toJS = function () {
        return ko.toJS({
            TodoItemId: self.todoItemId,
            Title: self.title,
            IsDone: self.isDone,
            TodoListId: self.todoListId
        });
    };
}

function todoList(dataModel, data) {
    var self = this;
    data = data || {};

    // convert raw todoItem data objects into array of TodoItems
    function importTodoItems(todoItems) {
        /// <returns value="[new todoItem()]"></returns>
        return $.map(todoItems || [],
                function (todoItemData) {
                    return new todoItem(dataModel, todoItemData);
                });
    }

    // Persisted properties
    self.todoListId = data.TodoListId;
    self.userId = data.UserId || "to be replaced";
    self.title = ko.observable(data.Title || "My todos");
    self.todos = ko.observableArray(importTodoItems(data.Todos));

    // Non-persisted properties
    self.isEditingListTitle = ko.observable(false);
    self.newTodoTitle = ko.observable();
    self.errorMessage = ko.observable();

    self.addTodo = function () {
        if (self.newTodoTitle()) { // need a title to save
            var item = new todoItem(
                dataModel,
                {
                    Title: self.newTodoTitle(),
                    TodoListId: self.todoListId
                });
            self.todos.push(item);
            item.errorMessage(null);
            dataModel.saveNewTodoItem(item.toJS())
                .done(function (result) {
                    item.todoItemId = result.TodoItemId;
                })
                .fail(function () {
                    item.errorMessage("Error adding a new todo item.");
                });
            self.newTodoTitle("");
        }
    };

    self.deleteTodo = function () {
        var todoItem = this;
        return dataModel.deleteTodoItem(todoItem.toJS())
            .done(function () {
                self.todos.remove(todoItem);
            })
            .fail(function () {
                todoItem.errorMessage("Error removing todo item.");
            });
    };

    // Auto-save when these properties change
    self.title.subscribe(function () {
        self.errorMessage(null);
        return dataModel.saveChangedTodoList(self.toJS())
            .fail(function () {
                self.errorMessage("Error updating the todo list title. Please make sure it is non-empty.");
            });
    });

    self.toJS = function () {
        return ko.toJS({
            TodoListId: self.todoListId,
            UserId: self.userId,
            Title: self.title
            // todos are not needed for updates
        });
    };
}
