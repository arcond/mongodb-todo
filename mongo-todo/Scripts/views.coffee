define [
	'jquery'
	'underscore'
	'backbone'
	'models'
	'collections'
], ($, _, Backbone, Models, Collections) ->
	class MainPage extends Backbone.View
		el: '#main-content'

		initialize: (options) ->
			@userId = options.userId if options?.userId
			if @userId then @setUser new Models.User id: @userId else @setUser new Models.User

			@users = new Collections.Users
			@todos = new Collections.Todos

			@toolbarView = new ToolbarView collection: @users
			@userView = new UserView model: @user
			@todoListView = new TodoListView collection: @todos
			
			@listenTo @users, 'reset', @renderToolbar
			@listenTo @todos, 'reset', @renderTodos
			@listenTo @todos, 'add', @renderTodos
			@listenTo @todos, 'remove', @renderTodos
			@listenTo @toolbarView, 'users:add', =>
				@setUser new Models.User
				@renderUser()
				return
			@listenTo @toolbarView, 'users:select', (userModel) =>
				if userModel
					@setUser userModel
				else
					@toolbarView.trigger 'users:add'
				return
			@listenTo @toolbarView, 'save-all', =>
				@todos.save() if @todos
				return

			super options

		render: ->
			@users.fetch()
			@

		renderToolbar: ->
			@toolbarView.$el.empty()
			@toolbarView.collection = @users
			@$el.html @toolbarView.render().el
			@toolbarView.setUser @userId
			@user.fetch() if @user.id and @userId isnt 0 and @userId isnt '0'
			@

		renderUser: ->
			@userView.$el.empty()
			@todoListView.$el.empty()
			@userView.model = @user
			@$el.append @userView.render().el
			@toolbarView.setUser @userId if @toolbarView
			@

		renderTodos: ->
			@todoListView.$el.empty()
			@todoListView.collection = @todos
			@$el.append @todoListView.render().el
			@renderTodo()
			@

		renderTodo: ->
			@todos.each (todoModel) =>
				todoModel.urlRoot = @user.references.TaskModels
				view = new TodoView model: todoModel
				@$el.find('ul.todos').append view.render().el
				return
			@renderNewTodo()
			@

		renderNewTodo: ->
			newTodo = new Models.Todo urlRoot: @user.references.TaskModels
			@listenTo newTodo, 'change:id', =>
				@todos.add newTodo
				return
			view = new TodoView model: newTodo
			@$el.find('ul.todos').append view.render().el
			@

		setUser: (user) ->
			if user
				@stopListening @user
				@user = user
				@listenTo @user, 'change:headers', =>
					@renderUser()
					@setTodos()
					return
				@userId = if @user.id then @user.id else 0
				unless user.id
					Backbone.history.navigate '#0', false
					@listenTo @user, 'change:id', =>
						@users.add @user
						@userId = @user.id
						Backbone.history.navigate "##{@userId}", false
						return
				else 
					@user.fetch() if @toolbarView
					Backbone.history.navigate "##{@user.id}", false
			return

		setTodos: ->
			if @user?.references?.TaskModels
				@todos.url = @user.references.TaskModels
				@todos.fetch()
			return

	class ToolbarView extends Backbone.View
		className: 'navbar navbar-fixed-top'
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

		addUser: (ev) ->
			ev.preventDefault() if ev?.preventDefault
			@trigger 'users:add'
			return

		setUser: (userId) ->
			@$el.find('#select-user>option[selected]').removeAttr 'selected'
			@$el.find("#select-user>option[value=#{userId}]").attr 'selected', 'selected'
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

	class UserView extends Backbone.View
		className: 'container'
		template: Handlebars.compile $('#user-template').html() ? ''
		events:
			'keyup #user-name': 'updateUserName'
			'click #save-user': 'saveUser'
			'click #edit-user': 'editUser'
			'click #update-user': 'saveUser'
			'click #cancel-user': 'render'

		render: ->
			if @model then @$el.html @template @model.toJSON() else @$el.html @template()
			super()

		updateUserName: (ev) ->
			@model.updateName @$el.find('input').val()
			return

		saveUser: (ev) ->
			ev.preventDefault() if ev?.preventDefault
			@model.save()
			return

		editUser: (ev) ->
			ev.preventDefault() if ev?.preventDefault
			@$el.html @template @model.toJSON edit: true
			return

	class TodoListView extends Backbone.View
		className: 'container'
		template: Handlebars.compile $('#todo-list-template').html() ? ''

		initialize: (options) ->
			if @collection
				@listenTo @collection, 'reset', @render
			super options

		render: ->
			@$el.html @template()
			super()

	class TodoView extends Backbone.View
		className: 'row'
		tagName: 'li'
		template: Handlebars.compile $('#todo-template').html() ? ''
		events:
			'keyup input.description': 'updateDescription'
			'click input[type=checkbox]': 'toggleComplete'
			'keyup input[type=checkbox]': 'toggleComplete'
			'click .add-task': 'addTask'
			'click .remove-task': 'removeTask'

		initialize: (options) ->
			@listenTo @model, 'change:completed', @render
			@listenTo @model, 'change:id', @render
			super options

		render: ->
			@$el.html @template @model.toJSON()
			super()

		updateDescription: (ev) ->
			@model.updateDescription $(ev.target).val()
			return

		addTask: (ev) ->
			ev.preventDefault() if ev?.preventDefault
			@model.save()
			return

		removeTask: (ev) ->
			ev.preventDefault() if ev?.preventDefault
			@model.destroy()
			return

		toggleComplete: ->
			@model.toggle()
			return

	{
		MainPage: MainPage
	}