define [
	'underscore'
	'backbone'
	'models'
], (_, Backbone, Models) ->
	class BaseCollection extends Backbone.Collection
		isDirty: ->
			unless @models then return false
			dirties = @filter (model) ->
				model.isDirty() is true if model.isDirty
			dirties.length > 0

		isValid: ->
			unless @models then return true
			invalids = @filter (model) ->
				model.isValid() is false if model.isValid
			invalids.length is 0

		save: ->
			models = @filter (model) ->
				model.isDirty() is true
			_.invoke models, 'save'
			return

	class Users extends BaseCollection
		model: Models.User
		url: '/api/users/all'

	class Todos extends BaseCollection
		model: Models.Todo
		url: ''

		completed: ->
			@filter (model) ->
				model.get('completed') is true

		remaining: ->
			@filter (model) ->
				model.get('completed') is false

	Collections = Collections ? {}
	Collections.Users
	Collections.Todos