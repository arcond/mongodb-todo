define [
	'underscore'
	'backbone'
	'collections'
], (_, Backbone, Collections) ->
	class BaseModel extends Backbone.Model
		@_preservedAttributes: {}

		constructor: (attributes, options) ->
			super attributes, options
			_preservedAttributes = @toJSON()
			return

		parse: (response) ->
			response = super response
			_preservedAttributes = @toJSON()
			response

		isDirty: ->
			not _.equals @toJSON(), _preservedAttributes

		set: (attributes, options) ->
			super attributes, options
			if @isValid() is false and not @has 'invalid'
				@set 'invalid', @isValid()
			else if @isValid() is true and @has 'invalid'
				@unset 'invalid',
					silent: true
			return

	class User extends BaseModel
		urlRoot: ''
		tasks: Collections.Todos
		defaults:
			name: ''

		initialize: (options) ->
			if options?.tasks
				tasks = new Collections.Todos options.tasks
			super options

		toJSON: ->
			json = super()
			json.tasks = tasks.toJSON()
			json

		updateName: (newName) ->
			@set
				name: newName

	class Todo extends BaseModel
		urlRoot: ''
		defaults:
			description: ''
			completed: false

		updateDescription: (newDescription) ->
			@set 'description', newDescription
			return

		toggle: ->
			@save 'completed', not @get 'completed'
			return

	Models = Models ? {}
	Models.User = User
	Models.Todo = Todo