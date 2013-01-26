// Generated by CoffeeScript 1.3.3
(function() {
  var __hasProp = {}.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

  define(['jquery', 'underscore', 'backbone', 'models', 'collections', 'views'], function($, _, Backbone, Models, Collections, Views) {
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
        this.$el = $('#main-content');
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
        this.toolbarView = new Views.ToolbarView({
          collection: this.users
        });
        this.listenTo(this.toolbarView, 'users:add', function() {
          Backbone.history.navigate("#0", true);
        });
        this.listenTo(this.toolbarView, 'users:select', function(userModel) {
          Backbone.history.navigate("#" + userModel.id, true);
        });
        this.listenTo(this.toolbarView, 'save-all', function() {
          _this.users.save();
        });
        this.$el.html(this.toolbarView.render().el);
        this.users.fetch();
      };

      Router.prototype.root = function() {
        this.setup();
      };

      Router.prototype.tasks = function(userId) {
        var _this = this;
        this.setup();
        this.userView.remove();
        this.listenTo(this.users, 'reset', function() {
          var user;
          if (userId && userId !== 0 && userId !== '0') {
            user = _this.users.get(userId);
          } else {
            user = new Models.User;
          }
          _this.userView = new Views.UserView({
            model: user
          });
          _this.$el.find('#user').html(_this.userView.render().el);
          if (userId && userId !== 0 && userId !== '0') {
            return _this.toolbarView.setUser(userId);
          }
        });
      };

      return Router;

    })(Backbone.Router);
    return {
      Router: Router
    };
  });

}).call(this);
