define [
	'jquery'
	'underscore'
	'backbone'
	'collections'
	'models'
], ($, _, Backbone, Collections, Models) ->
	class BaseView extends Backbone.View
		subviews: []

		constructor: (options) ->
			@subviews = []
			super options

		render: ->
			@

		remove: ->
			@trigger 'removed', @
			@removeSubViews()
			@off()
			@undelegateEvents()
			@$el.fadeOut 'fast', ->
				$(@).remove()
				return
			return

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

		isValid: ->
			if @collection?.isValid then return @collection.isValid()
			if @model?.isValid then return @model.isValid()
			true

		isDirty: ->
			if @collection?.isDirty then return @collection.isDirty()
			if @model?.isDirty then return @model.isDirty()
			false

		@_removeSubView: (view) ->
			@subviews = _.without @subviews, view
			return

	class ToolbarView extends BaseView
		template: ''
		events:
			'click #add-user': 'addUser'
			'change #select-user': 'selectUser'
			'click #save': 'save'

		initialize: (options) ->
			@collection.on 'reset', @render, @
			@collection.on 'add', @render, @
			@collection.on 'remove', @render, @
			@collection.on 'change', @render, @
			super options

		render: ->
			@removeSubViews()
			@$el.html @template @collection.toJSON()
			if @collection.isValid() and @collection.isDirty() then $('#save').enable()
			else $('#save').disable()
			super()

		addUser: ->
			@trigger 'users:add'
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
		template: ''
		events:
			'keyup #user-name': 'updateUserName'

		initialize: (options) ->
			@model.on 'change', @render, @
			@tasks = new Collections.Todos url: @model.get('tasksUrl')
			@tasks.on 'reset', @renderList, @
			super options

		render: ->
			@removeSubViews()
			@$el.html @template @model.toJSON()
			super()

		renderList: ->
			if @model.tasks?.length
				list = new TodoList collection: @tasks
				@addSubView list
			return

		updateUserName: (ev) ->
			@model.updateName $(ev.target).val()
			return

	class TodoList extends BaseView
		template: ''

		initialize: (options) ->
			@collection.on 'reset', @render, @
			@collection.on 'add', @render, @
			@collection.on 'remove', @render, @
			super options

		render: ->
			@$el.html @template()
			@renderRows()
			super()

		renderRows: ->
			@removeSubViews()
			@collection.filter (model) ->
				view = new TodoView model: model
				@addSubView view
				return
			return

	class TodoView extends BaseView
		template: ''
		events:
			'keyup input[type=text]': 'updateDescription'
			'click input[type=checkbox]': 'toggleComplete'
			'keyup input[type=checkbox]': 'toggleComplete'

		initialize: (options) ->
			@model.on 'change', @render, @
			super options

		render: ->
			@removeSubViews()
			@$el.html @template @model.toJSON()
			super()

		updateDescription: (ev) ->
			@model.updateDescription $(ev.target).val()
			return

		toggleComplete: ->
			@model.toggle()
			return

	Views = Views ? {}
	Views.ToolbarView
	Views.UserView