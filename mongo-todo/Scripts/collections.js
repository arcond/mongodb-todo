(function() {
  var __hasProp = {}.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

  define(['underscore', 'backbone', 'models'], function(_, Backbone, Models) {
    var BaseCollection, Collections, Todos, Users;
    BaseCollection = (function(_super) {

      __extends(BaseCollection, _super);

      function BaseCollection() {
        return BaseCollection.__super__.constructor.apply(this, arguments);
      }

      BaseCollection.prototype.isDirty = function() {
        var dirties;
        if (!this.models) {
          return false;
        }
        dirties = this.filter(function(model) {
          if (model.isDirty) {
            return model.isDirty() === true;
          }
        });
        return dirties.length > 0;
      };

      BaseCollection.prototype.isValid = function() {
        var invalids;
        if (!this.models) {
          return true;
        }
        invalids = this.filter(function(model) {
          if (model.isValid) {
            return model.isValid() === false;
          }
        });
        return invalids.length === 0;
      };

      BaseCollection.prototype.save = function() {
        var models;
        models = this.filter(function(model) {
          return model.isDirty() === true;
        });
        _.invoke(models, 'save');
      };

      return BaseCollection;

    })(Backbone.Collection);
    Users = (function(_super) {

      __extends(Users, _super);

      function Users() {
        return Users.__super__.constructor.apply(this, arguments);
      }

      Users.prototype.model = Models.User;

      Users.prototype.url = '';

      return Users;

    })(BaseCollection);
    Todos = (function(_super) {

      __extends(Todos, _super);

      function Todos() {
        return Todos.__super__.constructor.apply(this, arguments);
      }

      Todos.prototype.model = Models.Todo;

      Todos.prototype.url = '';

      Todos.prototype.completed = function() {
        return this.filter(function(model) {
          return model.get('completed') === true;
        });
      };

      Todos.prototype.remaining = function() {
        return this.filter(function(model) {
          return model.get('completed') === false;
        });
      };

      return Todos;

    })(BaseCollection);
    Collections = Collections != null ? Collections : {};
    Collections.Users;
    return Collections.Todos;
  });

}).call(this);
