define [
	'jquery'
	'underscore'
	'backbone'
	'models'
	'collections'
	'views'
], ($, _, Backbone, Models, Collections, Views) ->
	class Router extends Backbone.Router
		$el: null
		routes:
			'': 'root'
			':userId': 'tasks'

		initialize: (options) ->
			@$el = $('#main-content')
			@users = new Collections.Users
			@toolbarView = new Views.ToolbarView collection: @users
			@userView = new Views.UserView
			super options

		setup: ->
			@toolbarView.remove()
			@toolbarView = new Views.ToolbarView collection: @users
			@listenTo @toolbarView, 'users:add', =>
				Backbone.history.navigate "#0", true
				return
			@listenTo @toolbarView, 'users:select', (userModel) =>
				Backbone.history.navigate "##{userModel.id}", true
				return
			@listenTo @toolbarView, 'save-all', =>
				@users.save()
				return
			@$el.html @toolbarView.render().el

			@users.fetch()
			return			

		root: ->
			@setup()
			return

		tasks: (userId) ->
			@setup()
			@userView.remove()
			@listenTo @users, 'reset', =>
				if userId and userId isnt 0 and userId isnt '0' then user = @users.get userId
				else user = new Models.User
				@userView = new Views.UserView model: user
				@$el.find('#user').html @userView.render().el
				@toolbarView.setUser userId if userId and userId isnt 0 and userId isnt '0'
			return

	{
		Router: Router
	}