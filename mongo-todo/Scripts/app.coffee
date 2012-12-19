require.config
	shim:
		'underscore':
			exports: '_'
		'backbone':
			deps: [
				'jquery'
				'underscore'
			]
			exports: 'Backbone'
	paths:
		jquery: 'lib/jquery.min'
		underscore: 'lib/underscore'
		backbone: 'lib/backbone'
	urlArgs: 'bust=v2' # This can be used to invalidate the cache
require [
	'routers'
], (Routers) ->
	new Routers.Router
	Backbone.history.start()
	return