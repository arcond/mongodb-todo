(function() {
  var __hasProp = {}.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

  define(['underscore', 'backbone', 'collections'], function(_, Backbone, Collections) {
    var BaseModel, Models, Todo, User;
    BaseModel = (function(_super) {

      __extends(BaseModel, _super);

      BaseModel._preservedAttributes = {};

      function BaseModel(attributes, options) {
        var _preservedAttributes;
        BaseModel.__super__.constructor.call(this, attributes, options);
        _preservedAttributes = this.toJSON();
        return;
      }

      BaseModel.prototype.parse = function(response) {
        var _preservedAttributes;
        response = BaseModel.__super__.parse.call(this, response);
        _preservedAttributes = this.toJSON();
        return response;
      };

      BaseModel.prototype.isDirty = function() {
        return !_.equals(this.toJSON(), _preservedAttributes);
      };

      BaseModel.prototype.set = function(attributes, options) {
        BaseModel.__super__.set.call(this, attributes, options);
        if (this.isValid() === false && !this.has('invalid')) {
          this.set('invalid', this.isValid());
        } else if (this.isValid() === true && this.has('invalid')) {
          this.unset('invalid', {
            silent: true
          });
        }
      };

      return BaseModel;

    })(Backbone.Model);
    User = (function(_super) {

      __extends(User, _super);

      function User() {
        return User.__super__.constructor.apply(this, arguments);
      }

      User.prototype.urlRoot = '';

      User.prototype.tasks = Collections.Todos;

      User.prototype.defaults = {
        name: ''
      };

      User.prototype.initialize = function(options) {
        var tasks;
        if (options != null ? options.tasks : void 0) {
          tasks = new Collections.Todos(options.tasks);
        }
        return User.__super__.initialize.call(this, options);
      };

      User.prototype.toJSON = function() {
        var json;
        json = User.__super__.toJSON.call(this);
        json.tasks = tasks.toJSON();
        return json;
      };

      User.prototype.updateName = function(newName) {
        return this.set({
          name: newName
        });
      };

      return User;

    })(BaseModel);
    Todo = (function(_super) {

      __extends(Todo, _super);

      function Todo() {
        return Todo.__super__.constructor.apply(this, arguments);
      }

      Todo.prototype.urlRoot = '';

      Todo.prototype.defaults = {
        description: '',
        completed: false
      };

      Todo.prototype.updateDescription = function(newDescription) {
        this.set('description', newDescription);
      };

      Todo.prototype.toggle = function() {
        this.save('completed', !this.get('completed'));
      };

      return Todo;

    })(BaseModel);
    Models = Models != null ? Models : {};
    Models.User = User;
    return Models.Todo = Todo;
  });

}).call(this);
