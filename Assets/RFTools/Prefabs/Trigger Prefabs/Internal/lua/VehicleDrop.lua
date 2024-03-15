behaviour("VehicleDrop")

function VehicleDrop:OnTriggered(context)
	local joint = self.gameObject.GetComponent(Joint)
	joint.connectedBody = context.vehicle.rigidbody

	self.context = context
	self.signalSender = self.gameObject.GetComponent(TriggerScriptedSignal)
	self.vehicle = context.vehicle
	self.checkDetachTime = Time.time + 1
end

function VehicleDrop:FixedUpdate()
	if(Time.time > self.checkDetachTime) then
		if(self.vehicle.isDead or self.vehicle.rigidbody.velocity.y >= -0.01) then
			self.signalSender.Send("OnLand", self.context)
			GameObject.Destroy(self.gameObject)
		end
	end
end