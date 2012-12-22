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
			@toolbarView.on 'users:add', =>
				Backbone.history.navigate "#0", true
				return
			, @
			@toolbarView.on 'users:select', (userModel) =>
				Backbone.history.navigate "##{userModel.id}", true
				return
			, @
			@toolbarView.on 'save-all', =>
				@users.save()
				return
			, @
			@$el.html @toolbarView.render().el

			@users.fetch()
			return			

		root: ->
			@setup()
			return

		tasks: (userId) ->
			@userView.remove()
			@users.on 'reset', =>
				if userId then user = @users.get userId
				else user = new Models.User
				if user.get('tasksUrl')
					tasks = new Collections.Todos url: user.get('tasksUrl')
					tasks.on 'reset', =>
						@userView = new Views.UserView model: user, tasks: tasks
						@$el.find('#user').html @userView.render().el
						return
					tasks.fetch()
				else
					@userView = new Views.UserView model: user
					@$el.find('#user').html @userView.render().el
				return
			, @
			@users.fetch()
			return

	{
		Router: Router
	}