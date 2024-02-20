import { FamilyMember } from "@/models/task.model";

const API_URL = import.meta.env.VITE_API_URL;

export const FamilyService = {
    getMembers: function() {
        return fetch(`${API_URL}/familyMembers`)
            .then<FamilyMember[]>(response => response.json())
    }
};