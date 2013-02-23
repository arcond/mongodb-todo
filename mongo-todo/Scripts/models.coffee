define [
	'underscore'
	'backbone'
], (_, Backbone) ->
	class BaseModel extends Backbone.Model
		constructor: (options) ->
			@references = {}
			super options
			return

		sync: (method, model, options) ->
			xhr = super method, model, options
			xhr.done (data) =>
				linkHeader = xhr.getResponseHeader 'Link'
				if linkHeader
					links = linkHeader.split ','
					@references = @references
					_.each links, (link) =>
						parts = link.split ';'

						#<(https?:\/\/)?([\dA-z\.-]+)(:[\d]+)?(\.([A-z\.]{2,6}))?([/\w#\.-]*)*\/?>; ?rel=[\w\.-\[\]]+; ?type="[\w\.-\/]+"
						#(https?:\/\/)?([\dA-z\.-]+)(:[\d]+)?(\.([A-z\.]{2,6}))?([/\w#\.-]*)*\/?
						#rel=[\w\.-\[\]]+
						#type="[\w\.-\/]+"
						url = parts[0].replace '<', ''
						url = url.replace '>', ''

						relParts = parts[1].split '='
						rel = relParts[1].replace '[]', 's'

						@references[rel] = url
						return
					@trigger 'change:headers'
				return
			xhr

	class User extends BaseModel
		urlRoot: '/api/users'
		defaults:
			name: ''
			tasksUrl: ''

		updateName: (newName) ->
			@set
				name: newName

	class Todo extends BaseModel
		defaults:
			description: ''
			completed: false

		initialize: (options) ->
			if options?.urlRoot
				@urlRoot = ->
					options.urlRoot
			super options

		updateDescription: (newDescription) ->
			@set 'description', newDescription
			return

		toggle: ->
			@save 'completed', not @get 'completed'
			return

	{
		User: User
		Todo: Todo
	}