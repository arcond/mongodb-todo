define [
	'jquery'
	'underscore'
	'backbone'
	'models'
	'collections'
], ($, _, Backbone, Models, Collections) ->
	class BaseView extends Backbone.View
		render: ->
			super()
			@trigger 'rendered'
			@

	class ParentView extends BaseView
		subviews: []

		constructor: (options) ->
			@subviews = []
			super options

		remove: ->
			@trigger 'removed', @
			@removeSubViews()
			@$el.fadeOut 'fast', ->
				$(@).remove()
				return
			super()

		addSubView: (view, insertMethod = 'append', targetSelector = null) ->
			@subviews.push view
			view.on 'removed', @_removeSubView, @
			unless targetSelector then @$el[insertMethod] view.render().el
			else @$el.find(targetSelector)[insertMethod] view.render().el
			@ # we return this to allow chaining

		removeSubViews: ->
			_.invoke @subviews, 'remove'
			@subviews = []
			return

		@_removeSubView: (view) ->
			@subviews = _.without @subviews, view
			return

	class MainView extends ParentView
		el: '#main-content'

		initialize: (options) ->
			@userId = options.userId if options?.userId
			@users = new Collections.Users
			@todos = new Collections.Todos
			@user = new Models.User
			@toolbarView = new ToolbarView collection: @users
			@userView = new UserView
			@todoListView = new TodoListView
			@listenTo @users, 'reset', =>
				@user = @users.get @userId
				@renderToolbar()
				return
			super options

		render: ->
			super()
			@users.fetch()
			@

		renderToolbar: ->
			@toolbarView.remove()
			@toolbarView = new ToolbarView collection: @users
			@listenTo @toolbarView, 'users:add', =>
				@user = new Models.User
				@renderUser()
				Backbone.history.navigate "#0", false
				return
			@listenTo @toolbarView, 'users:select', (userModel) =>
				@user = userModel
				@renderUser()
				Backbone.history.navigate "##{@user.id}", false
				return
			@listenTo @toolbarView, 'save-all', =>
				@users.save()
				@todos.save() if @todos
				return
			@addSubView @toolbarView, 'html'
			@toolbarView.setUser @user.id if @user?.id
			@renderUser() if @userId
			@

		renderUser: ->
			@userView.remove()
			@userView = new UserView model: @user
			@listenTo @user, 'reset', =>
				Backbone.history.navigate "##{@user.id}", true
				return
			@listenTo @userView, 'rendered', =>
				@todos = new Collections.Todos url: @user.get('tasksUrl')
				@listenTo @todos, 'reset', @renderTodos
				@renderTodos()
				@todos.fetch()
				return
			@addSubView @userView, 'html', '#user'
			@

		renderTodos: ->
			@todoListView.remove()
			@todoListView = new TodoListView collection: @todos
			@addSubView @todoListView
			@listenTo @todoListView, 'rendered', @renderTodo
			@

		renderTodo: ->
			@todos.each (todoModel) =>
				view = new TodoView model: todoModel
				@addSubView view, 'append', 'ul'
				return
			view = new TodoView model: new Models.Todo(urlRoot: @todos.url, userId: @user.id)
			@addSubView view, 'append', 'ul'
			@


	class ToolbarView extends BaseView
		template: Handlebars.compile $('#toolbar-template').html() ? ''
		events:
			'click #create-user': 'addUser'
			'change #select-user': 'selectUser'
			'click #save': 'save'

		initialize: (options) ->
			@listenTo @collection, 'reset', @render
			@listenTo @collection, 'add', @render
			@listenTo @collection, 'remove', @render
			@listenTo @collection, 'change', @render
			super options

		render: ->
			@$el.html @template @collection.toJSON()
			super()

		addUser: ->
			@trigger 'users:add'
			return

		setUser: (userId) ->
			@$el.find("#select-user > option[value=#{userId}]").attr 'selected', 'selected'
			return

		selectUser: (ev) ->
			@trigger 'users:select', @collection.get $(ev.target).val()
			return

		save: ->
			@trigger 'save-all'
			return

		toggleNew: (allow) ->
			if allow is true then $('#add-user').enable()
			else $('#add-user').disable()
			return

	class UserView extends BaseView
		template: Handlebars.compile $('#user-template').html() ? ''
		events:
			'keyup #user-name': 'updateUserName'
			'click #save-user': 'saveUser'

		render: ->
			if @model then @$el.html @template @model.toJSON() else @$el.html @template()
			super()

		updateUserName: (ev) ->
			@model.updateName $(ev.target).val()
			return

		saveUser: (ev) ->
			ev.preventDefault() if ev?.preventDefault
			@model.save()
			return

	class TodoListView extends BaseView
		template: Handlebars.compile $('#todo-list-template').html() ? ''

		initialize: (options) ->
			if @collection
				@listenTo @collection, 'reset', @render
				@listenTo @collection, 'add', @render
				@listenTo @collection, 'remove', @render
			super options

		render: ->
			@$el.html @template()
			super()

	class TodoView extends BaseView
		template: Handlebars.compile $('#todo-template').html() ? ''
		events:
			'keyup input.description': 'updateDescription'
			'click input[type=checkbox]': 'toggleComplete'
			'keyup input[type=checkbox]': 'toggleComplete'
			'click .add-task': 'addTask'

		initialize: (options) ->
			@listenTo @model, 'change:completed', @render
			super options

		render: ->
			@$el.html @template @model.toJSON()
			super()

		updateDescription: (ev) ->
			console.log $(ev.target).val()
			@model.updateDescription $(ev.target).val()
			return

		addTask: (ev) ->
			ev.preventDefault() if ev?.preventDefault
			@model.save()
			return

		toggleComplete: ->
			@model.toggle()
			return

	{
		MainView: MainView
	}