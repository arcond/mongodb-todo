reuire.config
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

require [
	'routers'
], (Router) ->
	new Router
	Backbone.history.start()
	return