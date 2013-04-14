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
			':userId': 'todos'	

		root: ->
			@mainPage = new Views.MainPage
			@mainPage.render()
			return

		todos: (userId) ->
			@mainPage = new Views.MainPage userId: userId
			@mainPage.render()
			return

	{
		Router: Router
	}