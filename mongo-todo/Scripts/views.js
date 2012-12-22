// Generated by CoffeeScript 1.3.3
(function() {
  var __hasProp = {}.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

  define(['jquery', 'underscore', 'backbone', 'models', 'collections'], function($, _, Backbone, Models, Collections) {
    var BaseView, TodoList, TodoView, ToolbarView, UserView;
    BaseView = (function(_super) {

      __extends(BaseView, _super);

      BaseView.prototype.subviews = [];

      function BaseView(options) {
        this.subviews = [];
        BaseView.__super__.constructor.call(this, options);
      }

      BaseView.prototype.render = function() {
        this.delegateEvents();
        return this;
      };

      BaseView.prototype.remove = function() {
        this.trigger('removed', this);
        this.removeSubViews();
        this.off();
        this.undelegateEvents();
        this.$el.fadeOut('fast', function() {
          $(this).remove();
        });
      };

      BaseView.prototype.addSubView = function(view, insertMethod, targetSelector) {
        if (insertMethod == null) {
          insertMethod = 'append';
        }
        if (targetSelector == null) {
          targetSelector = null;
        }
        this.subviews.push(view);
        view.on('removed', this._removeSubView, this);
        if (!targetSelector) {
          this.$el[insertMethod](view.render().el);
        } else {
          this.$el.find(targetSelector)[insertMethod](view.render().el);
        }
        return this;
      };

      BaseView.prototype.removeSubViews = function() {
        _.invoke(this.subviews, 'remove');
        this.subviews = [];
      };

      BaseView.prototype.isValid = function() {
        var _ref, _ref1;
        if ((_ref = this.collection) != null ? _ref.isValid : void 0) {
          return this.collection.isValid();
        }
        if ((_ref1 = this.model) != null ? _ref1.isValid : void 0) {
          return this.model.isValid();
        }
        return true;
      };

      BaseView.prototype.isDirty = function() {
        var _ref, _ref1;
        if ((_ref = this.collection) != null ? _ref.isDirty : void 0) {
          return this.collection.isDirty();
        }
        if ((_ref1 = this.model) != null ? _ref1.isDirty : void 0) {
          return this.model.isDirty();
        }
        return false;
      };

      BaseView._removeSubView = function(view) {
        this.subviews = _.without(this.subviews, view);
      };

      return BaseView;

    })(Backbone.View);
    ToolbarView = (function(_super) {
      var _ref;

      __extends(ToolbarView, _super);

      function ToolbarView() {
        return ToolbarView.__super__.constructor.apply(this, arguments);
      }

      ToolbarView.prototype.template = Handlebars.compile((_ref = $('#toolbar-template').html()) != null ? _ref : '');

      ToolbarView.prototype.events = {
        'click #create-user': 'addUser',
        'change #select-user': 'selectUser',
        'click #save': 'save'
      };

      ToolbarView.prototype.initialize = function(options) {
        this.collection.on('reset', this.render, this);
        this.collection.on('add', this.render, this);
        this.collection.on('remove', this.render, this);
        this.collection.on('change', this.render, this);
        return ToolbarView.__super__.initialize.call(this, options);
      };

      ToolbarView.prototype.render = function() {
        this.removeSubViews();
        this.$el.html(this.template(this.collection.toJSON()));
        if (this.collection.isValid() && this.collection.isDirty()) {
          $('#save').enable();
        } else {
          $('#save').disable();
        }
        return ToolbarView.__super__.render.call(this);
      };

      ToolbarView.prototype.addUser = function() {
        this.trigger('users:add');
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

    })(BaseView);
    UserView = (function(_super) {
      var _ref;

      __extends(UserView, _super);

      function UserView() {
        return UserView.__super__.constructor.apply(this, arguments);
      }

      UserView.prototype.template = Handlebars.compile((_ref = $('#user-template').html()) != null ? _ref : '');

      UserView.prototype.events = {
        'keyup #user-name': 'updateUserName'
      };

      UserView.prototype.initialize = function(options) {
        if (options != null ? options.model : void 0) {
          this.model = options.model;
          this.model.on('change', this.render, this);
          if (this.model.get('tasksUrl')) {
            this.tasks = new Collections.Todos({
              url: this.model.get('tasksUrl')
            });
            this.tasks.on('reset', this.renderList, this);
            this.tasks.fetch();
          }
        }
        return UserView.__super__.initialize.call(this, options);
      };

      UserView.prototype.render = function() {
        this.removeSubViews();
        if (this.model) {
          this.$el.html(this.template(this.model.toJSON()));
        } else {
          this.$el.html(this.template());
        }
        return UserView.__super__.render.call(this);
      };

      UserView.prototype.renderList = function() {
        var list, _ref1;
        if ((_ref1 = this.tasks) != null ? _ref1.length : void 0) {
          list = new TodoList({
            collection: this.tasks
          });
          this.addSubView(list);
        }
      };

      UserView.prototype.updateUserName = function(ev) {
        this.model.updateName($(ev.target).val());
      };

      return UserView;

    })(BaseView);
    TodoList = (function(_super) {
      var _ref;

      __extends(TodoList, _super);

      function TodoList() {
        return TodoList.__super__.constructor.apply(this, arguments);
      }

      TodoList.prototype.template = Handlebars.compile((_ref = $('#todo-list-template').html()) != null ? _ref : '');

      TodoList.prototype.initialize = function(options) {
        this.collection.on('reset', this.render, this);
        this.collection.on('add', this.render, this);
        this.collection.on('remove', this.render, this);
        return TodoList.__super__.initialize.call(this, options);
      };

      TodoList.prototype.render = function() {
        this.$el.html(this.template());
        this.renderRows();
        return TodoList.__super__.render.call(this);
      };

      TodoList.prototype.renderRows = function() {
        var _this = this;
        this.removeSubViews();
        this.collection.filter(function(model) {
          var view;
          view = new TodoView({
            model: model
          });
          _this.addSubView(view, 'append', 'ul');
        });
      };

      return TodoList;

    })(BaseView);
    TodoView = (function(_super) {
      var _ref;

      __extends(TodoView, _super);

      function TodoView() {
        return TodoView.__super__.constructor.apply(this, arguments);
      }

      TodoView.prototype.template = Handlebars.compile((_ref = $('#todo-template').html()) != null ? _ref : '');

      TodoView.prototype.events = {
        'keyup input[type=text]': 'updateDescription',
        'click input[type=checkbox]': 'toggleComplete',
        'keyup input[type=checkbox]': 'toggleComplete'
      };

      TodoView.prototype.initialize = function(options) {
        this.model.on('change', this.render, this);
        return TodoView.__super__.initialize.call(this, options);
      };

      TodoView.prototype.render = function() {
        this.removeSubViews();
        this.$el.html(this.template(this.model.toJSON()));
        return TodoView.__super__.render.call(this);
      };

      TodoView.prototype.updateDescription = function(ev) {
        this.model.updateDescription($(ev.target).val());
      };

      TodoView.prototype.toggleComplete = function() {
        this.model.toggle();
      };

      return TodoView;

    })(BaseView);
    return {
      ToolbarView: ToolbarView,
      UserView: UserView
    };
  });

}).call(this);
