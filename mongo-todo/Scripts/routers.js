(function() {
  var __hasProp = {}.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

  define(['jquery', 'underscore', 'backbone', 'views', 'collections', 'models'], function($, _, Backbone, Views, Collections, Models) {
    var Router;
    Router = (function(_super) {

      __extends(Router, _super);

      function Router() {
        return Router.__super__.constructor.apply(this, arguments);
      }

      Router.prototype.$el = null;

      Router.prototype.routes = {
        '': 'root',
        ':userId': 'tasks'
      };

      Router.prototype.initialize = function(options) {
        this.$el = $('#content');
        this.users = new Collections.Users;
        this.toolbarView = new Views.ToolbarView({
          collection: this.users
        });
        this.userView = new Views.UserView;
        return Router.__super__.initialize.call(this, options);
      };

      Router.prototype.setup = function() {
        var _this = this;
        this.toolbarView.remove();
        this.toolbarView.on('users:add', function() {
          Backbone.history.navigate("#0", true);
        }, this);
        this.toolbarView.on('users:select', function(userModel) {
          Backbone.history.navigate("#" + userModel.id, true);
        }, this);
        this.toolbarView.on('save-all', function() {
          _this.users.save();
        }, this);
        this.$el.html(this.toolbarView.render().el);
        this.users.fetch();
      };

      Router.prototype.root = function() {
        this.setup();
      };

      Router.prototype.tasks = function(userId) {
        var _this = this;
        this.userView.remove();
        this.users.on('reset', function() {
          var user;
          if (userId > 0) {
            user = _this.users.get(userId);
          } else {
            user = new Models.User;
          }
          _this.userView = new Views.UserView({
            model: user
          });
          _this.$el.find('#user').html(_this.userView.render().el);
        }, this);
        this.setup();
      };

      return Router;

    })(Backbone.Router);
    return Router;
  });

}).call(this);