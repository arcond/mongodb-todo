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

      Router.prototype.routes = {
        '': 'root',
        ':userId': 'tasks'
      };

      Router.prototype.root = function() {
        this.mainPage = new Views.MainPage;
        this.mainPage.render();
      };

      Router.prototype.tasks = function(userId) {
        console.log(userId);
        this.mainPage = new Views.MainPage({
          userId: userId
        });
        this.mainPage.render();
      };

      return Router;

    })(Backbone.Router);
    return {
      Router: Router
    };
  });

}).call(this);
