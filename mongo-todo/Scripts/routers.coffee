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
			@mainPage = new Views.MainPage
			super options		

		root: ->
			@mainPage.removeSubViews()
			@mainPage = new Views.MainPage
			@mainPage.render()
			return

		tasks: (userId) ->
			@mainPage.removeSubViews()
			@mainPage = new Views.MainPage userId: userId
			@mainPage.render()
			return

	{
		Router: Router
	}