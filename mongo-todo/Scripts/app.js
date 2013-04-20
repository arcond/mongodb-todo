// Generated by CoffeeScript 1.6.2
(function() {
  require.config({
    shim: {
      'underscore': {
        exports: '_'
      },
      'backbone': {
        deps: ['jquery', 'underscore'],
        exports: 'Backbone'
      },
      'backbone.rest': {
        deps: ['backbone']
      }
    },
    paths: {
      jquery: 'lib/jquery.min',
      underscore: 'lib/underscore',
      backbone: 'lib/backbone'
    },
    urlArgs: 'bust=v2'
  });

  require(['routers', 'backbone.rest'], function(Routers) {
    new Routers.Router;
    Backbone.history.start();
  });

}).call(this);
