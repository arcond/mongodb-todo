define [
	'jquery'
	'underscore'
	'backbone'
	'models'
	'collections'
	'views'
], ($, _, Backbone, Models, Collections, Views) ->
	class Router extends Backbone.Router
		routes:
			'': 'root'
			':userId': 'tasks'

		initialize: (options) ->
			@masterView = new Views.MainView
			super options		

		root: ->
			@masterView.removeSubViews()
			@masterView = new Views.MainView
			@masterView.render()
			return

		tasks: (userId) ->
			@masterView.removeSubViews()
			@masterView = new Views.MainView userId: userId
			@masterView.render()
			return

	{
		Router: Router
	}