// Generated by CoffeeScript 1.3.3
(function() {
  var __hasProp = {}.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

  define(['underscore', 'backbone'], function(_, Backbone) {
    var Todo, User;
    User = (function(_super) {

      __extends(User, _super);

      function User() {
        return User.__super__.constructor.apply(this, arguments);
      }

      User.prototype.urlRoot = '/api/users';

      User.prototype.defaults = {
        name: ''
      };

      User.prototype.updateName = function(newName) {
        return this.set({
          name: newName
        });
      };

      User.prototype.toJSON = function(options) {
        var json;
        json = User.__super__.toJSON.call(this);
        if (options != null ? options.edit : void 0) {
          json.editing = true;
        }
        return json;
      };

      return User;

    })(Backbone.Model);
    Todo = (function(_super) {

      __extends(Todo, _super);

      function Todo() {
        return Todo.__super__.constructor.apply(this, arguments);
      }

      Todo.prototype.defaults = {
        description: '',
        completed: false
      };

      Todo.prototype.initialize = function(options) {
        if (options != null ? options.urlRoot : void 0) {
          this.urlRoot = function() {
            return options.urlRoot;
          };
        }
        return Todo.__super__.initialize.call(this, options);
      };

      Todo.prototype.updateDescription = function(newDescription) {
        this.set('description', newDescription);
      };

      Todo.prototype.toggle = function() {
        this.save('completed', !this.get('completed'));
      };

      return Todo;

    })(Backbone.Model);
    return {
      User: User,
      Todo: Todo
    };
  });

}).call(this);
