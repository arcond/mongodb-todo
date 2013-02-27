// Generated by CoffeeScript 1.3.3
(function() {
  var __hasProp = {}.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

  define(['jquery', 'underscore', 'backbone', 'models', 'collections'], function($, _, Backbone, Models, Collections) {
    var MainPage, TodoListView, TodoView, ToolbarView, UserView;
    MainPage = (function(_super) {

      __extends(MainPage, _super);

      function MainPage() {
        return MainPage.__super__.constructor.apply(this, arguments);
      }

      MainPage.prototype.el = '#main-content';

      MainPage.prototype.initialize = function(options) {
        var _this = this;
        if (options != null ? options.userId : void 0) {
          this.userId = options.userId;
        }
        if (this.userId) {
          this.setUser(new Models.User({
            id: this.userId
          }));
        } else {
          this.setUser(new Models.User);
        }
        this.users = new Collections.Users;
        this.todos = new Collections.Todos;
        this.toolbarView = new ToolbarView({
          collection: this.users
        });
        this.userView = new UserView({
          model: this.user
        });
        this.todoListView = new TodoListView({
          collection: this.todos
        });
        this.listenTo(this.users, 'reset', this.renderToolbar);
        this.listenTo(this.todos, 'reset', this.renderTodos);
        this.listenTo(this.todos, 'add', this.renderTodos);
        this.listenTo(this.todos, 'remove', this.renderTodos);
        this.listenTo(this.toolbarView, 'users:add', function() {
          _this.setUser(new Models.User);
          _this.renderUser();
        });
        this.listenTo(this.toolbarView, 'users:select', function(userModel) {
          if (userModel) {
            _this.setUser(userModel);
          } else {
            _this.toolbarView.trigger('users:add');
          }
        });
        this.listenTo(this.toolbarView, 'save-all', function() {
          if (_this.todos) {
            _this.todos.save();
          }
        });
        return MainPage.__super__.initialize.call(this, options);
      };

      MainPage.prototype.render = function() {
        this.users.fetch();
        return this;
      };

      MainPage.prototype.renderToolbar = function() {
        this.toolbarView.$el.empty();
        this.toolbarView.collection = this.users;
        this.$el.html(this.toolbarView.render().el);
        this.toolbarView.setUser(this.userId);
        if (this.user.id && this.userId !== 0 && this.userId !== '0') {
          this.user.fetch();
        }
        return this;
      };

      MainPage.prototype.renderUser = function() {
        this.userView.$el.empty();
        this.todoListView.$el.empty();
        this.userView.model = this.user;
        this.$el.append(this.userView.render().el);
        if (this.toolbarView) {
          this.toolbarView.setUser(this.userId);
        }
        return this;
      };

      MainPage.prototype.renderTodos = function() {
        this.todoListView.$el.empty();
        this.todoListView.collection = this.todos;
        this.$el.append(this.todoListView.render().el);
        this.renderTodo();
        return this;
      };

      MainPage.prototype.renderTodo = function() {
        var _this = this;
        this.todos.each(function(todoModel) {
          var view;
          todoModel.urlRoot = _this.user.references.TaskModels;
          view = new TodoView({
            model: todoModel
          });
          _this.$el.find('ul.todos').append(view.render().el);
        });
        this.renderNewTodo();
        return this;
      };

      MainPage.prototype.renderNewTodo = function() {
        var newTodo, view,
          _this = this;
        newTodo = new Models.Todo({
          urlRoot: this.user.references.TaskModels
        });
        this.listenTo(newTodo, 'change:id', function() {
          _this.todos.add(newTodo);
        });
        view = new TodoView({
          model: newTodo
        });
        this.$el.find('ul.todos').append(view.render().el);
        return this;
      };

      MainPage.prototype.setUser = function(user) {
        var _this = this;
        if (user) {
          this.stopListening(this.user);
          this.user = user;
          this.listenTo(this.user, 'change:headers', function() {
            _this.renderUser();
            _this.setTodos();
          });
          this.userId = this.user.id ? this.user.id : 0;
          if (!user.id) {
            Backbone.history.navigate('#0', false);
            this.listenTo(this.user, 'change:id', function() {
              _this.users.add(_this.user);
              _this.userId = _this.user.id;
              Backbone.history.navigate("#" + _this.userId, false);
            });
          } else {
            if (this.toolbarView) {
              this.user.fetch();
            }
            Backbone.history.navigate("#" + this.user.id, false);
          }
        }
      };

      MainPage.prototype.setTodos = function() {
        var _ref, _ref1;
        if ((_ref = this.user) != null ? (_ref1 = _ref.references) != null ? _ref1.TaskModels : void 0 : void 0) {
          this.todos.url = this.user.references.TaskModels;
          this.todos.fetch();
        }
      };

      return MainPage;

    })(Backbone.View);
    ToolbarView = (function(_super) {
      var _ref;

      __extends(ToolbarView, _super);

      function ToolbarView() {
        return ToolbarView.__super__.constructor.apply(this, arguments);
      }

      ToolbarView.prototype.className = 'navbar navbar-fixed-top';

      ToolbarView.prototype.template = Handlebars.compile((_ref = $('#toolbar-template').html()) != null ? _ref : '');

      ToolbarView.prototype.events = {
        'click #create-user': 'addUser',
        'change #select-user': 'selectUser',
        'click #save': 'save'
      };

      ToolbarView.prototype.initialize = function(options) {
        this.listenTo(this.collection, 'reset', this.render);
        this.listenTo(this.collection, 'add', this.render);
        this.listenTo(this.collection, 'remove', this.render);
        this.listenTo(this.collection, 'change', this.render);
        return ToolbarView.__super__.initialize.call(this, options);
      };

      ToolbarView.prototype.render = function() {
        this.$el.html(this.template(this.collection.toJSON()));
        return ToolbarView.__super__.render.call(this);
      };

      ToolbarView.prototype.addUser = function(ev) {
        if (ev != null ? ev.preventDefault : void 0) {
          ev.preventDefault();
        }
        this.trigger('users:add');
      };

      ToolbarView.prototype.setUser = function(userId) {
        this.$el.find('#select-user>option[selected]').removeAttr('selected');
        this.$el.find("#select-user>option[value=" + userId + "]").attr('selected', 'selected');
      };

      ToolbarView.prototype.selectUser = function(ev) {
        this.trigger('users:select', this.collection.get($(ev.target).val()));
      };

      ToolbarView.prototype.save = function() {
        this.trigger('save-all');
      };

      ToolbarView.prototype.toggleNew = function(allow) {
        if (allow === true) {
          $('#add-user').enable();
        } else {
          $('#add-user').disable();
        }
      };

      return ToolbarView;

    })(Backbone.View);
    UserView = (function(_super) {
      var _ref;

      __extends(UserView, _super);

      function UserView() {
        return UserView.__super__.constructor.apply(this, arguments);
      }

      UserView.prototype.className = 'container';

      UserView.prototype.template = Handlebars.compile((_ref = $('#user-template').html()) != null ? _ref : '');

      UserView.prototype.events = {
        'keyup #user-name': 'updateUserName',
        'click #save-user': 'saveUser'
      };

      UserView.prototype.render = function() {
        if (this.model) {
          this.$el.html(this.template(this.model.toJSON()));
        } else {
          this.$el.html(this.template());
        }
        return UserView.__super__.render.call(this);
      };

      UserView.prototype.updateUserName = function(ev) {
        this.model.updateName($(ev.target).val());
      };

      UserView.prototype.saveUser = function(ev) {
        if (ev != null ? ev.preventDefault : void 0) {
          ev.preventDefault();
        }
        this.model.save();
      };

      return UserView;

    })(Backbone.View);
    TodoListView = (function(_super) {
      var _ref;

      __extends(TodoListView, _super);

      function TodoListView() {
        return TodoListView.__super__.constructor.apply(this, arguments);
      }

      TodoListView.prototype.className = 'container';

      TodoListView.prototype.template = Handlebars.compile((_ref = $('#todo-list-template').html()) != null ? _ref : '');

      TodoListView.prototype.initialize = function(options) {
        if (this.collection) {
          this.listenTo(this.collection, 'reset', this.render);
        }
        return TodoListView.__super__.initialize.call(this, options);
      };

      TodoListView.prototype.render = function() {
        this.$el.html(this.template());
        return TodoListView.__super__.render.call(this);
      };

      return TodoListView;

    })(Backbone.View);
    TodoView = (function(_super) {
      var _ref;

      __extends(TodoView, _super);

      function TodoView() {
        return TodoView.__super__.constructor.apply(this, arguments);
      }

      TodoView.prototype.className = 'row';

      TodoView.prototype.tagName = 'li';

      TodoView.prototype.template = Handlebars.compile((_ref = $('#todo-template').html()) != null ? _ref : '');

      TodoView.prototype.events = {
        'keyup input.description': 'updateDescription',
        'click input[type=checkbox]': 'toggleComplete',
        'keyup input[type=checkbox]': 'toggleComplete',
        'click .add-task': 'addTask',
        'click .remove-task': 'removeTask'
      };

      TodoView.prototype.initialize = function(options) {
        this.listenTo(this.model, 'change:completed', this.render);
        this.listenTo(this.model, 'change:id', this.render);
        return TodoView.__super__.initialize.call(this, options);
      };

      TodoView.prototype.render = function() {
        this.$el.html(this.template(this.model.toJSON()));
        return TodoView.__super__.render.call(this);
      };

      TodoView.prototype.updateDescription = function(ev) {
        this.model.updateDescription($(ev.target).val());
      };

      TodoView.prototype.addTask = function(ev) {
        if (ev != null ? ev.preventDefault : void 0) {
          ev.preventDefault();
        }
        this.model.save();
      };

      TodoView.prototype.removeTask = function(ev) {
        if (ev != null ? ev.preventDefault : void 0) {
          ev.preventDefault();
        }
        this.model.destroy();
      };

      TodoView.prototype.toggleComplete = function() {
        this.model.toggle();
      };

      return TodoView;

    })(Backbone.View);
    return {
      MainPage: MainPage
    };
  });

}).call(this);
